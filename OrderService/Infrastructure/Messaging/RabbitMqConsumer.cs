using System.Text;
using System.Text.Json;
using MediatR;
using OrderService.Application.Orders.Commands.MarkOrderAsFailed;
using OrderService.Application.Orders.Commands.MarkOrderAsPaid;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Data;

namespace OrderService.Infrastructure.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private IMediator _mediator;
    private IConnection _connection;
    private IChannel _channel;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMqConsumer(IConfiguration config, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
        await _channel.QueueDeclareAsync("payment_processed_queue");
        await _channel.QueueBindAsync("payment_processed_queue", "payment_processed", "");
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var jsonOpt = new JsonSerializerOptions()
                      {
                          PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                      };
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, args) =>
                                  {
                                      using var scope = _serviceProvider.CreateScope();
                                      _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                      var json = Encoding.UTF8.GetString(args.Body.ToArray());
                                      var eventName = args.BasicProperties.Type;

                                      Result<bool> res = Result<bool>.Failure("Empty");
                                      
                                      switch(eventName)
                                      {
                                          case "payment_completed":
                                              var paidCommand = JsonSerializer.Deserialize<MarkOrderAsPaidCommand>(json, jsonOpt);
                                              res = await _mediator.Send(paidCommand, stoppingToken);
                                              break;
                                          case "payment_failed":
                                              var failedCommand = JsonSerializer.Deserialize<MarkOrderAsFailedCommand>(json, jsonOpt);
                                              res = await _mediator.Send(failedCommand, stoppingToken);
                                              break;
                                      }

                                      if (!res.IsSuccess)
                                      {
                                          await Console.Out.WriteLineAsync(res.ErrorMessage);
                                      }
                                  };
        await _channel.BasicConsumeAsync("payment_processed_queue", true, consumer, cancellationToken: stoppingToken);
    }
}