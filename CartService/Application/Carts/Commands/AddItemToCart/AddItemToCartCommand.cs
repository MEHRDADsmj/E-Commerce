namespace CartService.Application.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand(Guid UserId, Guid ProductId, int Quantity);