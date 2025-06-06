using OrderService.Contracts.Events;

namespace OrderService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(OrderCreatedEvent orderCreatedEvent);
}