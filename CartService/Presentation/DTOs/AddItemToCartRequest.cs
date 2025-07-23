using System.ComponentModel.DataAnnotations;

namespace CartService.Presentation.DTOs;

public class AddItemToCartRequestDto
{
    public Guid ProductId { get; }
    [Range(1, int.MaxValue)] public int Quantity { get; }

    public AddItemToCartRequestDto(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}