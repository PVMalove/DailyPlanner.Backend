using System.Text;
using DailyPlanner.Domain.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DailyPlanner.Consumer;

public class RabbitMqListener : BackgroundService
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly IOptions<RabbitMqSettings> options;


    public RabbitMqListener(IOptions<RabbitMqSettings> options)
    {
        this.options = options;
        var factory = new ConnectionFactory {HostName = "localhost"};
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.QueueDeclare(queue: options.Value.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stkppingToken)
    {
        stkppingToken.ThrowIfCancellationRequested();
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Received {message}");
            channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
        };
        channel.BasicConsume(queue: options.Value.QueueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }
}