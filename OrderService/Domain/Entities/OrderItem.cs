using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Entities;

public class OrderItem
{
    [Key] public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    [MaxLength(64)] public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    [Range(1, int.MaxValue)] public int Quantity { get; private set; }

    public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}