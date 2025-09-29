using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation
{
    public class MemberCreateValidator : AbstractValidator<MemberCreateDto>
    {
        public MemberCreateValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        }
    }

    public class MemberUpdateValidator : AbstractValidator<MemberUpdateDto>
    {
        public MemberUpdateValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        }
    }
}
