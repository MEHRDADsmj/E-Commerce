namespace OrderService.Contracts.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }

    public OrderCreatedEvent(Guid orderId, Guid userId, decimal totalPrice)
    {
        OrderId = orderId;
        UserId = userId;
        TotalPrice = totalPrice;
    }
}