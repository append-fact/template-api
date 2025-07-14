
using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Shared.RealTimeCommunication
{
    public class RealTimeCommunicationService : IRealTimeCommunicationService
    {
        private readonly ILogger<RealTimeCommunicationService> _logger;
        private readonly IHubContext<RealTimeHub> _hubContext;

        public RealTimeCommunicationService(
            ILogger<RealTimeCommunicationService> logger,
            IHubContext<RealTimeHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task BroadcastAsync<T>(string eventName, T payload)
        {
            _logger.LogInformation("Emisión de evento SignalR: {EventName} | Payload: {@Payload}", eventName, payload);
            await _hubContext.Clients.All.SendAsync(eventName, payload);
        }
    }
}
