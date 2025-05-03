using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messaging;

namespace PaymentService.MessageBus;

public class RabbitMQConsumer
{
    private readonly IConfiguration _config;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQConsumer(IConfiguration config)
    {
        _config = config;
    }

    private async Task InitRabbitMQ(ConnectionFactory factory)
    {
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public async Task StartConsumer()
    {
        ConnectionFactory factory = new ConnectionFactory()
                                    {
                                        HostName = _config["RabbitMQ:Host"],
                                        UserName = _config["RabbitMQ:User"],
                                        Password = _config["RabbitMQ:Password"],
                                        Port = int.Parse(_config["RabbitMQ:Port"]),
                                    };
        await InitRabbitMQ(factory);
        await _channel.ExchangeDeclareAsync("orders", ExchangeType.Fanout);
        var queueName = (await _channel.QueueDeclareAsync()).QueueName;
        await _channel.QueueBindAsync(queueName, "orders", "");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
                                  {
                                      await Task.Yield();
                                      var body = ea.Body.ToArray();
                                      var message = Encoding.UTF8.GetString(body);
                                      var obj = JsonSerializer.Deserialize<OrderCreatedEvent>(message)!;
                                      Console.WriteLine($" [x] Received {obj.GetType().Name} with values: \n" +
                                                        $"OrderId: {obj.OrderId}\t Amount: {obj.Amount}\t UserId: {obj.UserId}");
                                  };

        await _channel.BasicConsumeAsync(queueName, true, consumer);
    }
}