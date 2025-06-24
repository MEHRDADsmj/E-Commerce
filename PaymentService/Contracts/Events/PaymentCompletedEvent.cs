using PaymentService.Application.Interfaces;

namespace PaymentService.Contracts.Events;

public class PaymentCompletedEvent : IEvent
{
    public Guid OrderId { get; private set; }

    public PaymentCompletedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}