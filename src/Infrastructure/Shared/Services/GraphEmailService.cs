using Application.Common.Interfaces;
using Application.Common.Mailing;
using Application.DTOs.Common;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Microsoft.Identity.Client;
using Shared.Mailing;
using System.Net.Http.Headers;


namespace Shared.Services
{
    public class GraphEmailService : IEmailService
    {
        private readonly MailSettings _settings;
        private readonly IConfidentialClientApplication _clientApp;

        public GraphEmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
            _clientApp = ConfidentialClientApplicationBuilder
                .Create(_settings.ClientId)
                .WithClientSecret(_settings.ClientSecret)
                .WithAuthority(new Uri(_settings.Authority))
                .Build();
        }

        public Task SendAsync(MailRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailAsync(EmailDTO request, CancellationToken cancellationToken)
        {
            var token = await GetAccessTokenAsync();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var graphClient = new GraphServiceClient(httpClient);

            var message = new Message
            {
                Subject = request.Subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = EnsureUtf8Html(request.Body)
                },

                ToRecipients = new List<Recipient>
                {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = request.To
                    }
                }
            }
            };
            await graphClient.Users[_settings.GraphEmailFrom]
                .SendMail.PostAsync(new SendMailPostRequestBody
                {
                    Message = message,
                    SaveToSentItems = true
                });
        }


        private async Task<string> GetAccessTokenAsync()
        {
            var result = await _clientApp.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                             .ExecuteAsync();
            return result.AccessToken;
        }

        private string EnsureUtf8Html(string html)
        {
            if (!html.Contains("charset"))
            {
                // Asegura que el <meta charset="UTF-8"> esté presente en el <head>
                var headTagIndex = html.IndexOf("<head", StringComparison.OrdinalIgnoreCase);
                if (headTagIndex >= 0)
                {
                    var headCloseIndex = html.IndexOf(">", headTagIndex);
                    if (headCloseIndex > headTagIndex)
                    {
                        return html.Insert(headCloseIndex + 1, "<meta charset=\"UTF-8\">");
                    }
                }
            }
            return html;
        }

    }

}
