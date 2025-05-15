using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.UpdateItemQuantity;

public record UpdateItemQuantityCommand(Guid UserId, Guid ProductId, int NewQuantity) : IRequest<Result<bool>>;