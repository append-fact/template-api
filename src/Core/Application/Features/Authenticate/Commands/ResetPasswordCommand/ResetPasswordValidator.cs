﻿using FluentValidation;

namespace Application.Features.Authenticate.Commands.ResetPasswordCommand
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
