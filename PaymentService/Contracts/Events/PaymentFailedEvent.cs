using PaymentService.Application.Interfaces;

namespace PaymentService.Contracts.Events;

public class PaymentFailedEvent : IEvent
{
    public Guid OrderId { get; private set; }

    public PaymentFailedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}