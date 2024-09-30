namespace DailyPlanner.Producer.Interfaces;

public interface IMessageProducer
{
    void SendMessage<T>(T message, string routeKey, string? exchange = default);
}