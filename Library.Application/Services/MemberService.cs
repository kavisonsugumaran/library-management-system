using AutoMapper;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class MemberService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<MemberReadDto>> GetAllAsync()
        {
            var memberRepository = _unitOfWork.Repository<Member>();
            var members = await memberRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemberReadDto>>(members);
        }

        public async Task<MemberReadDto?> GetAsync(int id)
        {
            var memberRepository = _unitOfWork.Repository<Member>();
            var member = await memberRepository.GetByIdAsync(id);
            return member is null ? null : _mapper.Map<MemberReadDto>(member);
        }

        public async Task<MemberReadDto> CreateAsync(MemberCreateDto memberCreateDto)
        {
            var member = _mapper.Map<Member>(memberCreateDto);
            var memberRepository = _unitOfWork.Repository<Member>();
            await memberRepository.AddAsync(member);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MemberReadDto>(member);
        }

        public async Task<bool> UpdateAsync(int id, MemberUpdateDto memberUpdateDto)
        {
            var memberRepository = _unitOfWork.Repository<Member>();
            var member = await memberRepository.GetByIdAsync(id);
            if (member is null) return false;
            _mapper.Map(memberUpdateDto, member);
            memberRepository.Update(member);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var memberRepository = _unitOfWork.Repository<Member>();
            var member = await memberRepository.GetByIdAsync(id);
            if (member is null) return false;
            memberRepository.Remove(member);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
