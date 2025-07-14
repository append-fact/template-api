using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using FluentAssertions;
using Moq;

namespace Template.Tests.UnitTests.Application.Features.Authenticate.Commands.AuthenticateCommandHandlerTest
{
    public class AuthenticateCommandHandlerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AuthenticateCommandHandler _handler;

        public AuthenticateCommandHandlerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _handler = new AuthenticateCommandHandler(_accountServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_CallsServiceAndReturnsResponse()
        {
            // Arrange
            var cmd = new AuthenticateCommand
            {
                Email = "user@example.com",
                Password = "Secret123!",
                IpAddress = "192.168.0.1"
            };

            var authResponse = new AuthenticationResponse
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user1",
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "user@example.com",
                IsVerified = true,
                JWToken = "jwt-token",
                UrlImage = new Uri("https://example.com/avatar.png"),
                IsActive = true,
                Roles = new List<string> { "Admin" },
                RoleClaims = new List<string> { "Create", "Read" },
                RefreshToken = "refresh-token",
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            };

            var expectedResponse = new Response<AuthenticationResponse>(
                data: authResponse,
                message: $"Usuario {authResponse.UserName} autenticado"
            );

            _accountServiceMock
                .Setup(s => s.AuthenticateAsync(
                    It.Is<AuthenticationRequest>(r =>
                        r.Email == cmd.Email &&
                        r.Password == cmd.Password),
                    cmd.IpAddress))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            _accountServiceMock.Verify(s => s.AuthenticateAsync(
                    It.Is<AuthenticationRequest>(r =>
                        r.Email == cmd.Email &&
                        r.Password == cmd.Password),
                    cmd.IpAddress),
                Times.Once);



            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Handle_ServiceThrowsApiException_PropagatesException()
        {
            // Arrange
            var cmd = new AuthenticateCommand
            {
                Email = "noexiste@example.com",
                Password = "wrongpass",
                IpAddress = "10.0.0.1"
            };

            _accountServiceMock
                .Setup(s => s.AuthenticateAsync(
                    It.IsAny<AuthenticationRequest>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new ApiException("No hay cuenta registrada con el Email noexiste@example.com"));

            // Act
            Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

            // Assert
            await act
                .Should()
                .ThrowAsync<ApiException>()
                .WithMessage("No hay cuenta registrada con el Email noexiste@example.com");
        }
    }
}
