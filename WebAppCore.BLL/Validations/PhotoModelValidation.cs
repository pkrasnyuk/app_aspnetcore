using FluentValidation;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Validations
{
    public class PhotoModelValidation : AbstractValidator<PhotoModel>
    {
        public PhotoModelValidation()
        {
            RuleFor(u => u.Title)
                .NotEmpty().WithMessage("Please ensure you have entered the Title");

            RuleFor(u => u.AlbumId)
                .NotEqual(string.Empty).WithMessage("Please ensure you have selected the Album");
        }
    }
}