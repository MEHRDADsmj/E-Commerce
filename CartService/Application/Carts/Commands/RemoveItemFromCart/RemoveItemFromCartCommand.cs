using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.RemoveItemFromCart;

public record RemoveItemFromCartCommand(Guid UserId, Guid ProductId) : IRequest<Result<bool>>;