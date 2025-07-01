using System.Text;
using System.Text.Json;
using MediatR;
using PaymentService.Application.Payments.Commands.HandleOrderCreated;
using PaymentService.Contracts.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentService.Infrastructure.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private IConnection _connection;
    private IChannel _channel;
    private readonly IMediator _mediator;

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
        await _channel.QueueDeclareAsync("order_created", false, true, true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var jsonOpt = new JsonSerializerOptions
                      {
                          PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                      };
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
                                  {
                                      var body = ea.Body;
                                      var message = Encoding.UTF8.GetString(body.ToArray());
                                      var eventObj = JsonSerializer.Deserialize<OrderCreatedEvent>(message, jsonOpt);
                                      if (eventObj == null)
                                      {
                                          await Console.Error.WriteLineAsync($"OrderCreatedEvent json is not valid");
                                          return;
                                      }
                                      var command = new HandleOrderCreatedCommand(eventObj.OrderId, eventObj.UserId, eventObj.TotalPrice);
                                      var res = await _mediator.Send(command, stoppingToken);
                                      if (!res.IsSuccess)
                                      {
                                          await Console.Error.WriteLineAsync("Failed to process order");
                                      }
                                  };
        await _channel.BasicConsumeAsync("order_created", true, consumer, stoppingToken);
    }
}