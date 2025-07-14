using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Utilities.Commands.SendRealTimeNotificationsCommand
{
    public class SendRealTimeNotificationsCommandHandler : IRequestHandler<SendRealTimeNotificationsCommand, Response<string>>
    {
        private readonly IRealTimeCommunicationService _realTimeService;

        public SendRealTimeNotificationsCommandHandler(IRealTimeCommunicationService realTimeService)
        {
            _realTimeService = realTimeService;
        }
        public async Task<Response<string>> Handle(SendRealTimeNotificationsCommand request, CancellationToken cancellationToken)
        {

            await _realTimeService.BroadcastAsync(request.EventName, request.Notification);

            return new Response<string>(true, "Notificación enviada correctamente.");
        }
    }
}
