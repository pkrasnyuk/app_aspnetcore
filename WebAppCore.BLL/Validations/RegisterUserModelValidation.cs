using System.Collections.Generic;
using FluentValidation;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Validations
{
    public class RegisterUserModelValidation : AbstractValidator<RegisterUserModel>
    {
        public RegisterUserModelValidation(IEnumerable<RegisterUserModel> items)
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Please ensure you have entered the UserName")
                .Length(2, 100).WithMessage("The UserName must have between 2 and 100 characters");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Please ensure you have entered the Email")
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the Password")
                .Length(2, 8).WithMessage("The Password must have between 2 and 8 characters");

            RuleFor(u => u.Email).SetValidator(new UniquePropertyValidator<RegisterUserModel>(items))
                .WithMessage("This Email has already been registered");
        }
    }
}