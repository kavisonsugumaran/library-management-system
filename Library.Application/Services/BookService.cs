using AutoMapper;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class BookService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<BookReadDto>> GetAllAsync(string? search = null)
        {
            var bookRepository = _unitOfWork.Repository<Book>();
            var bookEntities = await bookRepository.GetAllAsync(
                filter: string.IsNullOrWhiteSpace(search) ? null : b => b.Title.Contains(search) || b.Isbn.Contains(search),
                orderBy: q => q.OrderBy(b => b.Title),
                includes: b => b.Author!);
            return _mapper.Map<IEnumerable<BookReadDto>>(bookEntities);
        }

        public async Task<BookReadDto?> GetAsync(int id)
        {
            var bookRepository = _unitOfWork.Repository<Book>();
            var bookEntity = await bookRepository.GetByIdAsync(id, b => b.Author!);
            return bookEntity is null ? null : _mapper.Map<BookReadDto>(bookEntity);
        }

        public async Task<BookReadDto> CreateAsync(BookCreateDto bookCreateDto)
        {
            var authorRepository = _unitOfWork.Repository<Author>();
            if (!await authorRepository.AnyAsync(a => a.Id == bookCreateDto.AuthorId))
                throw new InvalidOperationException("Author not found.");

            var bookEntity = _mapper.Map<Book>(bookCreateDto);
            var bookRepository = _unitOfWork.Repository<Book>();
            await bookRepository.AddAsync(bookEntity);
            await _unitOfWork.SaveChangesAsync();
            var createdBookEntity = await bookRepository.GetByIdAsync(bookEntity.Id, b => b.Author!);
            return _mapper.Map<BookReadDto>(createdBookEntity);
        }

        public async Task<bool> UpdateAsync(int id, BookUpdateDto bookUpdateDto)
        {
            var bookRepository = _unitOfWork.Repository<Book>();
            var bookEntity = await bookRepository.GetByIdAsync(id);
            if (bookEntity is null) return false;

            if (bookUpdateDto.AvailableCopies > bookUpdateDto.TotalCopies)
                throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

            _mapper.Map(bookUpdateDto, bookEntity);
            bookRepository.Update(bookEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bookRepository = _unitOfWork.Repository<Book>();
            var bookEntity = await bookRepository.GetByIdAsync(id);
            if (bookEntity is null) return false;
            bookRepository.Remove(bookEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
