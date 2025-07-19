using System.ComponentModel.DataAnnotations;

namespace OrderService.Contracts.Entities;

public class CartItem
{
    public Guid ProductId { get; init; }
    [Range(1, int.MaxValue)] public int Quantity { get; init; }

    public CartItem()
    {
        
    }

    public CartItem(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }
}