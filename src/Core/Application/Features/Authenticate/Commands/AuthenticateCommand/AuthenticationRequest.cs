namespace Application.Features.Authenticate.Commands.AuthenticateCommand
{
    public class AuthenticationRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}