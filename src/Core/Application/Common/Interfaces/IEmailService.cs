using Application.Common.Mailing;
using Application.DTOs.Common;

namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(MailRequest request, CancellationToken cancellationToken);
        Task SendEmailAsync(EmailDTO request, CancellationToken cancellationToken);
    }
}
