namespace Shared.Messaging;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
}