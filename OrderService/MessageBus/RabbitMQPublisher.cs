using System.Text;
using RabbitMQ.Client;

namespace OrderService.MessageBus;

public class RabbitMQPublisher
{
    private readonly IConfiguration _config;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQPublisher(IConfiguration config)
    {
        _config = config;
        ConnectionFactory factory = new ConnectionFactory()
                                    {
                                        HostName = _config["RabbitMQ:Host"],
                                        UserName = _config["RabbitMQ:User"],
                                        Password = _config["RabbitMQ:Password"],
                                        Port = int.Parse(_config["RabbitMQ:Port"]),
                                    };
        InitRabbitMQ(factory);
    }

    private async Task InitRabbitMQ(ConnectionFactory factory)
    {
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public async Task PublishMessage(string message)
    {
        await _channel.ExchangeDeclareAsync("orders", ExchangeType.Fanout);
        var body = Encoding.UTF8.GetBytes(message);
        await _channel.BasicPublishAsync("orders", "", true, body);
    }
}