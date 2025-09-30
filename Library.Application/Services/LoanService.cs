using AutoMapper;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Services
{
    public class LoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanReadDto>> GetAllAsync()
        {
            var loanRepository = _unitOfWork.Repository<Loan>();
            var loanEntities = await loanRepository.GetAllAsync(includes: [l => l.Book, l => l.Member]);
            return _mapper.Map<IEnumerable<LoanReadDto>>(loanEntities);
        }

        public async Task<LoanReadDto?> GetAsync(int id)
        {
            var loanRepository = _unitOfWork.Repository<Loan>();
            var loanEntity = await loanRepository.GetByIdAsync(id, l => l.Book, l => l.Member);
            return loanEntity is null ? null : _mapper.Map<LoanReadDto>(loanEntity);
        }

        public async Task<LoanReadDto> CreateAsync(LoanCreateDto loanCreateDto)
        {
            var bookRepository = _unitOfWork.Repository<Book>();
            var memberRepository = _unitOfWork.Repository<Member>();
            var loanRepository = _unitOfWork.Repository<Loan>();

            var bookEntity = await bookRepository.GetByIdAsync(loanCreateDto.BookId);
            if (bookEntity is null) throw new InvalidOperationException("Book not found.");
            if (bookEntity.AvailableCopies <= 0) throw new InvalidOperationException("No copies available.");

            if (!await memberRepository.AnyAsync(m => m.Id == loanCreateDto.MemberId))
                throw new InvalidOperationException("Member not found.");

            bookEntity.AvailableCopies -= 1;
            bookRepository.Update(bookEntity);

            var loanEntity = new Loan
            {
                BookId = bookEntity.Id,
                MemberId = loanCreateDto.MemberId,
                DueDate = loanCreateDto.DueDate ?? DateTime.UtcNow.AddDays(14),
                Status = LoanStatus.Active
            };

            await loanRepository.AddAsync(loanEntity);
            await _unitOfWork.SaveChangesAsync();

            var createdLoanEntity = await loanRepository.GetByIdAsync(loanEntity.Id, l => l.Book, l => l.Member);
            return _mapper.Map<LoanReadDto>(createdLoanEntity);
        }

        public async Task<bool> ReturnAsync(int id, LoanReturnDto loanReturnDto)
        {
            var loanRepository = _unitOfWork.Repository<Loan>();
            var bookRepository = _unitOfWork.Repository<Book>();

            var loanEntity = await loanRepository.GetByIdAsync(id);
            if (loanEntity is null) return false;
            if (loanEntity.Status == LoanStatus.Returned) return true;

            loanEntity.Status = LoanStatus.Returned;
            loanEntity.ReturnDate = loanReturnDto.ReturnDate;

            var bookEntity = await bookRepository.GetByIdAsync(loanEntity.BookId);
            if (bookEntity != null)
            {
                bookEntity.AvailableCopies += 1;
                if (bookEntity.AvailableCopies > bookEntity.TotalCopies) bookEntity.AvailableCopies = bookEntity.TotalCopies;
                bookRepository.Update(bookEntity);
            }

            loanRepository.Update(loanEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var loanRepository = _unitOfWork.Repository<Loan>();
            var loanEntity = await loanRepository.GetByIdAsync(id);
            if (loanEntity is null) return false;
            loanRepository.Remove(loanEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
