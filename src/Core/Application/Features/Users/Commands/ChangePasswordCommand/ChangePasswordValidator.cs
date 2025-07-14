using FluentValidation;

namespace Application.Features.Users.Commands.ChangePasswordCommand
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(p => p.Password)
            .NotEmpty().WithMessage("El campo no puede estar vacío.");

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("El campo no puede estar vacío.");

            RuleFor(p => p.ConfirmNewPassword)
                .NotEmpty().WithMessage("El campo no puede estar vacío.")
                .Equal(p => p.NewPassword)
                    .WithMessage("Las contraseñas no coinciden.");
        }
        
    }
}
