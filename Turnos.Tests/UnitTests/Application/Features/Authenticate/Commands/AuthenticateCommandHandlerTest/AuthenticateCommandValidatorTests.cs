using Application.Features.Authenticate.Commands.AuthenticateCommand;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Template.Tests.UnitTests.Application.Features.Authenticate.Validators
{

    namespace Template.Tests.UnitTests.Application.Features.Authenticate.Commands.AuthenticateCommandHandlerTest
    {
        public class AuthenticateCommandValidatorTests
        {
            private readonly AuthenticateCommandValidator _validator = new();

            [Theory]
            [InlineData("user@example.com", "Secret123!")]
            [InlineData("foo.bar@domain.co", "abcdef1")]
            public void Validate_ValidValues_NoErrors(string email, string password)
            {
                var cmd = new AuthenticateCommand { Email = email, Password = password };
                var result = _validator.TestValidate(cmd);
                result.IsValid.Should().BeTrue();
            }

            [Fact]
            public void Validate_EmptyEmail_HasError()
            {
                var cmd = new AuthenticateCommand { Email = "", Password = "Secret123!" };
                var result = _validator.TestValidate(cmd);
                result.ShouldHaveValidationErrorFor(c => c.Email)
                      .WithErrorMessage("El email no puede estar vacío.");
            }

            [Fact]
            public void Validate_InvalidEmailFormat_HasError()
            {
                var cmd = new AuthenticateCommand { Email = "no-valid@", Password = "Secret123!" };
                var result = _validator.TestValidate(cmd);
                result.ShouldHaveValidationErrorFor(c => c.Email)
                      .WithErrorMessage("El email debe tener un formato válido.");
            }

            [Fact]
            public void Validate_EmptyPassword_HasError()
            {
                var cmd = new AuthenticateCommand { Email = "user@example.com", Password = "" };
                var result = _validator.TestValidate(cmd);
                result.ShouldHaveValidationErrorFor(c => c.Password)
                      .WithErrorMessage("La contraseña no puede estar vacía.");
            }

            [Fact]
            public void Validate_PasswordTooShort_HasError()
            {
                var cmd = new AuthenticateCommand { Email = "user@example.com", Password = "123" };
                var result = _validator.TestValidate(cmd);
                result.ShouldHaveValidationErrorFor(c => c.Password)
                      .WithErrorMessage("La contraseña debe tener al menos 6 caracteres.");
            }

            [Fact]
            public void Validate_PasswordTooLong_HasError()
            {
                var longPwd = new string('x', 101);
                var cmd = new AuthenticateCommand { Email = "user@example.com", Password = longPwd };
                var result = _validator.TestValidate(cmd);
                result.ShouldHaveValidationErrorFor(c => c.Password)
                      .WithErrorMessage("La contraseña no debe exceder los 100 caracteres.");
            }
        }
    }
}
