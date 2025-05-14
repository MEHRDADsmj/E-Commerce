namespace CartService.Application.Carts.Commands.RemoveItemFromCart;

public record RemoveItemFromCartCommand(Guid UserId, Guid ProductId);