namespace Application.Common.Interfaces
{
    public interface IRealTimeCommunicationService
    {
        Task BroadcastAsync<T>(string eventName, T payload);
    }
}
 