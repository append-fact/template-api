using Application.Common.Interfaces;
using Application.Common.Mailing;
using Application.DTOs.Common;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.Mailing;

namespace Shared.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly MailSettings _settings;

        public SmtpEmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public Task SendAsync(MailRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailAsync(EmailDTO request, CancellationToken cancellationToken)
        {
            var message = new MimeMessage();


            message.From.Add(new MailboxAddress("Template App", _settings.SmtpEmailFrom));

            // Destinatario
            message.To.Add(new MailboxAddress("", request.To));

            // Asunto
            message.Subject = request.Subject;


            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = request.Body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Si deseas ignorar la validación de certificados (no recomendado en producción):
                // client.ServerCertificateValidationCallback = (s, c, h, e) => true;


                await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls, cancellationToken);

                // Autenticar con tu correo y la contraseña de aplicación que generaste
                await client.AuthenticateAsync(_settings.SmtpEmailFrom, _settings.SmtpAppPassword, cancellationToken);

                // Enviar el mensaje
                await client.SendAsync(message, cancellationToken);

                // Desconectar
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}
