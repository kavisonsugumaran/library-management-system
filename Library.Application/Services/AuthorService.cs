using AutoMapper;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<AuthorReadDto>> GetAllAsync()
        {
            var authorRepository = _unitOfWork.Repository<Author>();
            var authors = await authorRepository.GetAllAsync(orderBy: q => q.OrderBy(a => a.LastName));
            return _mapper.Map<IEnumerable<AuthorReadDto>>(authors);
        }

        public async Task<AuthorReadDto?> GetAsync(int id)
        {
            var authorRepository = _unitOfWork.Repository<Author>();
            var author = await authorRepository.GetByIdAsync(id);
            return author is null ? null : _mapper.Map<AuthorReadDto>(author);
        }

        public async Task<AuthorReadDto> CreateAsync(AuthorCreateDto dto)
        {
            var author = _mapper.Map<Author>(dto);
            var authorRepository = _unitOfWork.Repository<Author>();
            await authorRepository.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AuthorReadDto>(author);
        }

        public async Task<bool> UpdateAsync(int id, AuthorUpdateDto dto)
        {
            var authorRepository = _unitOfWork.Repository<Author>();
            var author = await authorRepository.GetByIdAsync(id);
            if (author is null) return false;
            _mapper.Map(dto, author);
            authorRepository.Update(author);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var authorRepository = _unitOfWork.Repository<Author>();
            var author = await authorRepository.GetByIdAsync(id);
            if (author is null) return false;
            authorRepository.Remove(author);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
    //public class AuthorService
    //{
    //    private readonly IUnitOfWork _uow;
    //    private readonly IMapper _mapper;

    //    public AuthorService(IUnitOfWork uow, IMapper mapper)
    //    {
    //        _uow = uow;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<AuthorReadDto>> GetAllAsync()
    //    {
    //        var items = await _uow.Repository<Author>().GetAllAsync(orderBy: q => q.OrderBy(a => a.LastName));
    //        return _mapper.Map<IEnumerable<AuthorReadDto>>(items);
    //    }

    //    public async Task<AuthorReadDto?> GetAsync(int id)
    //    {
    //        var item = await _uow.Repository<Author>().GetByIdAsync(id);
    //        return item is null ? null : _mapper.Map<AuthorReadDto>(item);
    //    }

    //    public async Task<AuthorReadDto> CreateAsync(AuthorCreateDto dto)
    //    {
    //        var entity = _mapper.Map<Author>(dto);
    //        await _uow.Repository<Author>().AddAsync(entity);
    //        await _uow.SaveChangesAsync();
    //        return _mapper.Map<AuthorReadDto>(entity);
    //    }

    //    public async Task<bool> UpdateAsync(int id, AuthorUpdateDto dto)
    //    {
    //        var repo = _uow.Repository<Author>();
    //        var entity = await repo.GetByIdAsync(id);
    //        if (entity is null) return false;
    //        _mapper.Map(dto, entity);
    //        repo.Update(entity);
    //        await _uow.SaveChangesAsync();
    //        return true;
    //    }

    //    public async Task<bool> DeleteAsync(int id)
    //    {
    //        var repo = _uow.Repository<Author>();
    //        var entity = await repo.GetByIdAsync(id);
    //        if (entity is null) return false;
    //        repo.Remove(entity);
    //        await _uow.SaveChangesAsync();
    //        return true;
    //    }
    //}
}
