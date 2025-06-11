using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Entities;

public class Order
{
    [Key] public Guid Id { get; set; }
    [Required] public Guid UserId { get; set; }
    [Required] public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Order(Guid userId, List<OrderItem> items)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Items = items;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        TotalPrice = items.Sum(item => item.UnitPrice * item.Quantity);
    }

    public Order()
    {
        Id = Guid.NewGuid();
        Status = OrderStatus.Pending;
        TotalPrice = 0;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void MarkAsPaid() => Status = OrderStatus.Paid;
    public void MarkAsFailed() => Status = OrderStatus.Failed;

    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }

    public void SetItems(List<OrderItem> items)
    {
        Items = items;
        TotalPrice = items.Sum(item => item.UnitPrice * item.Quantity);
    }
}