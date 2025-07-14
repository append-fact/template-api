using FluentValidation;

namespace Application.Common.Storage
{
    public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
    {
        public FileUploadRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(p => p.Extension)
                .NotEmpty()
                .MaximumLength(5);

            RuleFor(p => p.Data)
                .NotEmpty();
        }
    }
}
