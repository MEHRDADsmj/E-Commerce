namespace CartService.Presentation.DTOs;

public record UpdateItemQuantityRequestDto(Guid ProductId, int NewQuantity);