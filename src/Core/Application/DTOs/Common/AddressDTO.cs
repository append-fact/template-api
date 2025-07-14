using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.Common
{
    public class AddressDTO
    {
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public int? Floor { get; set; }
        public string? Apartment { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? ZipCode { get; set; }
    }


    public class AddressDTOValidator : AbstractValidator<AddressDTO>
    {
        public AddressDTOValidator()
        {
            // Validación de la calle
            RuleFor(p => p.Street)
                .NotEmpty().WithMessage("La calle no puede estar vacía.")
                .MaximumLength(250).WithMessage("La calle no debe exceder los {MaxLength} caracteres.");

            // Validación del número de casa
            RuleFor(p => p.HouseNumber)
                .NotEmpty().WithMessage("El número de casa no puede estar vacío.")
                .GreaterThan(0).WithMessage("El número de casa debe ser mayor a cero.");

            
            RuleFor(p => p.Floor)
                .GreaterThanOrEqualTo(0).When(p => p.Floor.HasValue).WithMessage("El piso debe ser un número válido.");

            
            RuleFor(p => p.Apartment)
                .MaximumLength(4).WithMessage("El departamento no debe exceder los {MaxLength} caracteres.");

            
            RuleFor(p => p.AdditionalInfo)
                .MaximumLength(255).WithMessage("La información adicional no debe exceder los {MaxLength} caracteres.");

            // Validación del código postal (Argentina, 8 dígitos con letras)
            RuleFor(p => p.ZipCode)
                .NotEmpty().WithMessage("El código postal no puede estar vacío.")
                .Matches(@"^[a-zA-Z0-9]{1,8}$").WithMessage("El código postal debe tener entre 1 y 8 caracteres y solo puede contener letras y números.");

        }
    }
}
