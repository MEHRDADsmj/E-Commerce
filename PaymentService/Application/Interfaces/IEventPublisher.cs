namespace PaymentService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(IEvent evt);
}