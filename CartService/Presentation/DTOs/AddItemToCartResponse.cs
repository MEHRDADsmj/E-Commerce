namespace CartService.Presentation.DTOs;

public record AddItemToCartResponseDto(Guid ProductId, int AddQuantity);