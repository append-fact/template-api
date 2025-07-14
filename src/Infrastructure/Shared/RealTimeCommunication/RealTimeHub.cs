using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Shared.RealTimeCommunication
{
    public class RealTimeHub : Hub
    {
        private readonly ILogger<RealTimeHub> _logger;

        public RealTimeHub(ILogger<RealTimeHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Cliente conectado: {ConnectionId}", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Cliente desconectado: {ConnectionId}", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            _logger.LogInformation("SendMessage invocado con: {Message}", message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
