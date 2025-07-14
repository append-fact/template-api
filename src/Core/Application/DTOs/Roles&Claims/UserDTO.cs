namespace Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? UrlImage { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string>? Roles { get; set; }


    }
}
