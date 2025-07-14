using System.Text.Json.Serialization;

namespace Application.Features.Authenticate.Commands.AuthenticateCommand
{
    public class AuthenticationResponse
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public string? JWToken { get; set; }
        public Uri? UrlImage { get; set; }
        public bool IsActive { get; set; }
        public List<string>? Roles { get; set; }
        public List<string>? RoleClaims { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime? RefreshTokenExpiration { get; set; }

    }
}