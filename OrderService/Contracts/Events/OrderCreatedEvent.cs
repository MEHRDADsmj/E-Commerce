namespace OrderService.Contracts.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public decimal TotalPrice { get; init; }

    public OrderCreatedEvent(Guid orderId, Guid userId, decimal totalPrice)
    {
        OrderId = orderId;
        UserId = userId;
        TotalPrice = totalPrice;
    }
}