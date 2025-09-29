using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation
{
    public class LoanCreateValidator : AbstractValidator<LoanCreateDto>
    {
        public LoanCreateValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0);
            RuleFor(x => x.MemberId).GreaterThan(0);
            // DueDate optional; if provided, must be future
            RuleFor(x => x.DueDate).Must(d => d == null || d > DateTime.UtcNow).WithMessage("DueDate must be in the future.");
        }
    }

    public class LoanReturnValidator : AbstractValidator<LoanReturnDto>
    {
        public LoanReturnValidator()
        {
            RuleFor(x => x.ReturnDate).LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(1));
        }
    }
}
