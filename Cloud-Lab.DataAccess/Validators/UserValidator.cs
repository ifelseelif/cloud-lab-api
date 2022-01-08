using Cloud_Lab.Entities.Requests;
using FluentValidation;

namespace Cloud_Lab.DataAccess.Validators
{
    public class UserValidator : AbstractValidator<UserCredential>
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