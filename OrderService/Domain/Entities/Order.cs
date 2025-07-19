using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Entities;

public class Order
{
    [Key] public Guid Id { get; init; }
    [Required] public Guid UserId { get; init; }
    [Required] public List<OrderItem> Items { get; init; }
    [Required, Range(0, double.MaxValue)] public decimal TotalPrice { get; private init; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; init; }

    public Order(Guid userId, List<OrderItem> items)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Items = items;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        TotalPrice = items.Sum(item => item.UnitPrice * item.Quantity);
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }

    public Order() : this(Guid.Empty, new List<OrderItem>())
    {
        
    }
    
    public void MarkAsPaid() => Status = OrderStatus.Paid;
    public void MarkAsFailed() => Status = OrderStatus.Failed;
}