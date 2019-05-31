using FluentValidation;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Validations
{
    public class AlbumModelValidation : AbstractValidator<AlbumModel>
    {
        public AlbumModelValidation()
        {
            RuleFor(u => u.Title)
                .NotEmpty().WithMessage("Please ensure you have entered the Title");

            RuleFor(u => u.Description)
                .NotEmpty().WithMessage("Please ensure you have entered the Description");
        }
    }
}