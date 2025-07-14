using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.Common
{
    public class PhoneDTO
    {
        public string? PhoneAreaCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class PhoneDTOValidator : AbstractValidator<PhoneDTO>
    {
        public PhoneDTOValidator()
        {
            RuleFor(p => p.PhoneAreaCode)
                .NotEmpty().WithMessage("El código de área no puede estar vacío.")
                .Matches(@"^\d{1,5}$").WithMessage("El código de área debe ser un número de entre 1 y 5 dígitos.");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("El número de teléfono no puede estar vacío.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("El número de teléfono debe ser un número válido.");
        }
    }
}
