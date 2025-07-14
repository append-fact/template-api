using Application.Common.Interfaces;
using Application.Common.Mailing;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Settings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Mailing;
using Shared.RealTimeCommunication;
using Shared.Services;
using Shared.Storage;

namespace Shared
{
    public static class ServiceExtensions
    {
        public static void AddSharedLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.Configure<OriginOptions>(configuration.GetSection(nameof(OriginOptions)));

            services
            .AddTransient<IMediator, Mediator>()
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<IDateTimeService, DateTimeService>()
            .AddTransient<IEmailService, GraphEmailService>()
            .AddTransient<IEmailTemplateService, EmailTemplateService>()
            .AddTransient<IStorageService, StorageService>()
            .AddTransient<IRealTimeCommunicationService, RealTimeCommunicationService>();
        }
    }
}