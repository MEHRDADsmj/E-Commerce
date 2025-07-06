using System.Text;
using System.Text.Json;
using PaymentService.Application.Interfaces;
using RabbitMQ.Client;

namespace PaymentService.Infrastructure.Messaging;

public class RabbitMqPublisher : IEventPublisher
{
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMqPublisher(IConfiguration config)
    {
        Init(config).Wait();
    }

    private async Task Init(IConfiguration config)
    {
        var factory = new ConnectionFactory()
                      {
                          HostName = config["RabbitMQ:Host"],
                          UserName = config["RabbitMQ:User"],
                          Password = config["RabbitMQ:Password"],
                          Port = int.Parse(config["RabbitMQ:Port"])
                      };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync("payment_processed", ExchangeType.Fanout);
    }

    public async Task PublishAsync<T>(T evt, string queueName)
    {
        var jsonOpt = new JsonSerializerOptions
                      {
                          PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                      };
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt, jsonOpt));
        var props = new BasicProperties()
                    {
                        Persistent = true,
                        Type = queueName
                    };
        await _channel.BasicPublishAsync("payment_processed", "payment_processed_queue", false, props, body);
    }
}