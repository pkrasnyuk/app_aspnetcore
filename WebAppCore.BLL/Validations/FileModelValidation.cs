using FluentValidation;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Validations
{
    public class FileModelValidation : AbstractValidator<FileModel>
    {
        public FileModelValidation()
        {
            RuleFor(u => u.FileName)
                .NotEmpty().WithMessage("Please ensure you have upload the File");

            RuleFor(u => u.Source)
                .NotNull().WithMessage("Please ensure you have upload the File");
        }
    }
}