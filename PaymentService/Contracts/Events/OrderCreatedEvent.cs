using PaymentService.Application.Interfaces;

namespace PaymentService.Contracts.Events;

public record OrderCreatedEvent(Guid OrderId, Guid UserId, decimal TotalPrice) : IEvent;