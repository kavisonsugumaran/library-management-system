using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation
{
    public class AuthorCreateValidator : AbstractValidator<AuthorCreateDto>
    {
        public AuthorCreateValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        }
    }

    public class AuthorUpdateValidator : AbstractValidator<AuthorUpdateDto>
    {
        public AuthorUpdateValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        }
    }
}
