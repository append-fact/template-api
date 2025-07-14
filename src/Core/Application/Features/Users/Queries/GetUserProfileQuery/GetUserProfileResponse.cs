namespace Application.Features.Users.Queries.GetUserProfileQuery
{
    public class GetUserProfileResponse
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsVerified { get; set; }
        public Uri? UrlImage { get; set; }
        public bool IsActive { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AreaNumber { get; set; }
        public DateTime? LastAccess { get; set; }

    }
}
