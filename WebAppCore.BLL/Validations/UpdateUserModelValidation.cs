using System.Collections.Generic;
using FluentValidation;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Validations
{
    public class UpdateUserModelValidation : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidation(IEnumerable<UpdateUserModel> items)
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Please ensure you have entered the UserName")
                .Length(2, 100).WithMessage("The UserName must have between 2 and 100 characters");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Please ensure you have entered the Email")
                .EmailAddress();

            RuleFor(u => u.Email).SetValidator(new UniquePropertyValidator<UpdateUserModel>(items))
                .WithMessage("This Email has already been registered");
        }
    }
}