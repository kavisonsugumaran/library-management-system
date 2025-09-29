using AutoMapper;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Services
{
    public class LoanService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanReadDto>> GetAllAsync()
        {
            var list = await _uow.Repository<Loan>().GetAllAsync(includes: [l => l.Book!, l => l.Member!]);
            return _mapper.Map<IEnumerable<LoanReadDto>>(list);
        }

        public async Task<LoanReadDto?> GetAsync(int id)
        {
            var entity = await _uow.Repository<Loan>().GetByIdAsync(id, l => l.Book!, l => l.Member!);
            return entity is null ? null : _mapper.Map<LoanReadDto>(entity);
        }

        public async Task<LoanReadDto> CreateAsync(LoanCreateDto dto)
        {
            var bookRepo = _uow.Repository<Book>();
            var memberRepo = _uow.Repository<Member>();
            var loanRepo = _uow.Repository<Loan>();

            var book = await bookRepo.GetByIdAsync(dto.BookId);
            if (book is null) throw new InvalidOperationException("Book not found.");
            if (book.AvailableCopies <= 0) throw new InvalidOperationException("No copies available.");

            if (!await memberRepo.AnyAsync(m => m.Id == dto.MemberId))
                throw new InvalidOperationException("Member not found.");

            book.AvailableCopies -= 1;
            bookRepo.Update(book);

            var loan = new Loan
            {
                BookId = book.Id,
                MemberId = dto.MemberId,
                DueDate = dto.DueDate ?? DateTime.UtcNow.AddDays(14),
                Status = LoanStatus.Active
            };

            await loanRepo.AddAsync(loan);
            await _uow.SaveChangesAsync();

            var created = await loanRepo.GetByIdAsync(loan.Id, l => l.Book!, l => l.Member!);
            return _mapper.Map<LoanReadDto>(created);
        }

        public async Task<bool> ReturnAsync(int id, LoanReturnDto dto)
        {
            var loanRepo = _uow.Repository<Loan>();
            var bookRepo = _uow.Repository<Book>();

            var loan = await loanRepo.GetByIdAsync(id);
            if (loan is null) return false;
            if (loan.Status == LoanStatus.Returned) return true;

            loan.Status = LoanStatus.Returned;
            loan.ReturnDate = dto.ReturnDate;

            var book = await bookRepo.GetByIdAsync(loan.BookId);
            if (book != null)
            {
                book.AvailableCopies += 1;
                if (book.AvailableCopies > book.TotalCopies) book.AvailableCopies = book.TotalCopies;
                bookRepo.Update(book);
            }

            loanRepo.Update(loan);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _uow.Repository<Loan>();
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return false;
            repo.Remove(entity);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
