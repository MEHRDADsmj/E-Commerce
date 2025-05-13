using System.ComponentModel.DataAnnotations;

namespace CartService.Domain.Entities;

public class Cart
{
    [Key] public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}