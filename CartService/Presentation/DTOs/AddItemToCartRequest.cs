namespace CartService.Presentation.DTOs;

public record AddItemToCartRequestDto(Guid ProductId, int Quantity);