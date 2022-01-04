using Api.Entities;
using FluentValidation;

namespace Api.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password can't be null or empty");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username can't be null or empty");
        }
    }
}