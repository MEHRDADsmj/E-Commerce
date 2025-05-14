namespace CartService.Application.Carts.Commands.UpdateItemQuantity;

public record UpdateItemQuantityCommand(Guid UserId, Guid ProductId, int NewQuantity);