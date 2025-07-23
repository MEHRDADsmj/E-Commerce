using System.ComponentModel.DataAnnotations;

namespace CartService.Presentation.DTOs;

public class UpdateItemQuantityRequestDto
{
    public Guid ProductId { get; }
    [Range(1, int.MaxValue)] public int NewQuantity { get; }

    public UpdateItemQuantityRequestDto(Guid productId, int newQuantity)
    {
        ProductId = productId;
        NewQuantity = newQuantity;
    }
}