using System.ComponentModel.DataAnnotations;

namespace OrderService.Contracts.Entities;

public class CartItem
{
    public Guid ProductId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}