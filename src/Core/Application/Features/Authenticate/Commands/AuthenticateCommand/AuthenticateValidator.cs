using FluentValidation;

namespace Application.Features.Authenticate.Commands.AuthenticateCommand
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email no puede estar vacío.")
                .EmailAddress().WithMessage("El email debe tener un formato válido.")
                .MaximumLength(250).WithMessage("El email no debe exceder los 250 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña no puede estar vacía.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
                .MaximumLength(100).WithMessage("La contraseña no debe exceder los 100 caracteres.");

        }
    }
}
