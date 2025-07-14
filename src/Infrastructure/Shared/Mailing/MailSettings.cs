namespace Shared.Mailing
{
    public class MailSettings
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? TenantId { get; set; }
        public string? Authority => $"https://login.microsoftonline.com/{TenantId}/v2.0";
        public string? GraphEmailFrom { get; set; }
        public string? SmtpEmailFrom { get; set; }
        public string? SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpAppPassword { get; set; }
    }
}
