using System.ComponentModel.DataAnnotations;

namespace OrderService.Contracts.Entities;

public class Cart
{
    [Key] public Guid UserId { get; init; }
    public List<CartItem> Items { get; init; }

    public Cart(Guid userId, List<CartItem> items)
    {
        UserId = userId;
        Items = items;
    }

    public bool IsEmpty() => UserId == Guid.Empty;
    
    public static Cart Empty()
    {
        return new Cart(Guid.Empty, new List<CartItem>());
    }
}