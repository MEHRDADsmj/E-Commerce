namespace OrderService.Contracts.Events;

public class PaymentCompletedEvent
{
    public Guid OrderId { get; set; }

    public PaymentCompletedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}