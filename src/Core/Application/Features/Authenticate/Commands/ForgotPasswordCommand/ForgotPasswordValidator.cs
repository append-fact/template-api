using FluentValidation;

namespace Application.Features.Authenticate.Commands.ForgotPasswordCommand
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
