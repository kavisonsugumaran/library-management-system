using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation
{
    public class BookCreateValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Isbn).NotEmpty().MaximumLength(13);
            RuleFor(x => x.AuthorId).GreaterThan(0);
            RuleFor(x => x.TotalCopies).GreaterThan(0);
        }
    }

    public class BookUpdateValidator : AbstractValidator<BookUpdateDto>
    {
        public BookUpdateValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Isbn).NotEmpty().MaximumLength(13);
            RuleFor(x => x.AuthorId).GreaterThan(0);
            RuleFor(x => x.TotalCopies).GreaterThan(0);
            RuleFor(x => x.AvailableCopies).GreaterThanOrEqualTo(0);
        }
    }
}
