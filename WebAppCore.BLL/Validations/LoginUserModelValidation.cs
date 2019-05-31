using FluentValidation;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Validations
{
    public class LoginUserModelValidation : AbstractValidator<LoginUserModel>
    {
        public LoginUserModelValidation()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Please ensure you have entered the Email")
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the Password")
                .Length(2, 8).WithMessage("The Password must have between 2 and 8 characters");
        }
    }
}