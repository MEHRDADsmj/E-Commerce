namespace CartService.Presentation.DTOs;

public record UpdateItemQuantityResponseDto(Guid ProductId, int NewQuantity);