namespace PaymentService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T evt, string queueName);
}