using System.ComponentModel.DataAnnotations;

namespace CartService.Domain.Entities;

public class Cart
{
    [Key] public Guid UserId { get; init; }
    public List<CartItem> Items { get; init; }
    
    public bool IsEmpty() => UserId == Guid.Empty;

    public Cart(Guid userId, List<CartItem> items)
    {
        UserId = userId;
        Items = items;
    }

    public static Cart Empty()
    {
        return new Cart(Guid.Empty, new List<CartItem>());
    }
}