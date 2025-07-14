namespace Application.Common.RazorModels
{
    public class ForgotPasswordEmailModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? RegistrationUrl { get; set; }
    }
}
