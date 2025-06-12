using System.Text;
using System.Text.Json;
using MediatR;
using OrderService.Application.Orders.Commands.MarkOrderAsPaid;
using OrderService.Contracts.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.Infrastructure.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IMediator _mediator;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMqConsumer(IConfiguration config, IMediator mediator)
    {
        _mediator = mediator;
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
        await _channel.QueueDeclareAsync("payment_completed", true, false, false, null);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, args) =>
                                  {
                                      var json = Encoding.UTF8.GetString(args.Body.ToArray());
                                      var evt = JsonSerializer.Deserialize<PaymentCompletedEvent>(json);

                                      if (evt is not null)
                                      {
                                          await _mediator.Send(new MarkOrderAsPaidCommand(evt.OrderId), stoppingToken);
                                      }
                                  };
        await _channel.BasicConsumeAsync("payment_completed", true, consumer, cancellationToken: stoppingToken);
    }
}