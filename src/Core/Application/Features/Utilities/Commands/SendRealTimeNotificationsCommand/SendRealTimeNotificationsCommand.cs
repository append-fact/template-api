using Application.Common.Wrappers;
using Application.DTOs.Common;
using MediatR;

namespace Application.Features.Utilities.Commands.SendRealTimeNotificationsCommand
{
    public class SendRealTimeNotificationsCommand : IRequest<Response<string>>
    {
        public string EventName { get; set; } = default!;
        public NotificationDTO Notification { get; set; } = default!;
    }
}
