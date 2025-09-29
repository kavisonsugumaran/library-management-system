using AutoMapper;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class BookService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookReadDto>> GetAllAsync(string? search = null)
        {
            var repo = _uow.Repository<Book>();
            var books = await repo.GetAllAsync(
                filter: string.IsNullOrWhiteSpace(search) ? null : b => b.Title.Contains(search) || b.Isbn.Contains(search),
                orderBy: q => q.OrderBy(b => b.Title),
                includes: b => b.Author!);
            return _mapper.Map<IEnumerable<BookReadDto>>(books);
        }

        public async Task<BookReadDto?> GetAsync(int id)
        {
            var book = await _uow.Repository<Book>().GetByIdAsync(id, b => b.Author!);
            return book is null ? null : _mapper.Map<BookReadDto>(book);
        }

        public async Task<BookReadDto> CreateAsync(BookCreateDto dto)
        {
            // Ensure Author exists
            if (!await _uow.Repository<Author>().AnyAsync(a => a.Id == dto.AuthorId))
                throw new InvalidOperationException("Author not found.");

            var entity = _mapper.Map<Book>(dto);
            await _uow.Repository<Book>().AddAsync(entity);
            await _uow.SaveChangesAsync();
            return _mapper.Map<BookReadDto>(await _uow.Repository<Book>().GetByIdAsync(entity.Id, b => b.Author!));
        }

        public async Task<bool> UpdateAsync(int id, BookUpdateDto dto)
        {
            var repo = _uow.Repository<Book>();
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return false;

            // keep available copies within 0..total
            if (dto.AvailableCopies > dto.TotalCopies)
                throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

            _mapper.Map(dto, entity);
            repo.Update(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _uow.Repository<Book>();
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return false;
            repo.Remove(entity);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
