using System.Text;
using System.Text.Json;
using OrderService.Application.Interfaces;
using OrderService.Contracts.Events;
using RabbitMQ.Client;

namespace OrderService.Infrastructure.Messaging;

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
        await _channel.ExchangeDeclareAsync("order_exchange", ExchangeType.Fanout, true);
    }
    
    public async Task PublishAsync(OrderCreatedEvent orderCreatedEvent)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderCreatedEvent));
        var props = new BasicProperties()
                    {
                        Persistent = true,
                    };
        string exchange = "order_exchange";
        await _channel.BasicPublishAsync(exchange, "", false, props, body);
    }
}