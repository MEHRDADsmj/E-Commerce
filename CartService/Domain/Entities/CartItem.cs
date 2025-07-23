using System.ComponentModel.DataAnnotations;

namespace CartService.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; init; }

    [Range(1, int.MaxValue)] private int _quantity;
    
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value > 0)
            {
                _quantity = value;
            }
        }
    }

    public CartItem(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }
}