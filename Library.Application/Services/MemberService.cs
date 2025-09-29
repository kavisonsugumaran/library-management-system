using AutoMapper;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class MemberService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberReadDto>> GetAllAsync()
        {
            var list = await _uow.Repository<Member>().GetAllAsync(orderBy: q => q.OrderBy(m => m.FullName));
            return _mapper.Map<IEnumerable<MemberReadDto>>(list);
        }

        public async Task<MemberReadDto?> GetAsync(int id)
        {
            var entity = await _uow.Repository<Member>().GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<MemberReadDto>(entity);
        }

        public async Task<MemberReadDto> CreateAsync(MemberCreateDto dto)
        {
            var entity = _mapper.Map<Member>(dto);
            await _uow.Repository<Member>().AddAsync(entity);
            await _uow.SaveChangesAsync();
            return _mapper.Map<MemberReadDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, MemberUpdateDto dto)
        {
            var repo = _uow.Repository<Member>();
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return false;
            _mapper.Map(dto, entity);
            repo.Update(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _uow.Repository<Member>();
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return false;
            repo.Remove(entity);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
