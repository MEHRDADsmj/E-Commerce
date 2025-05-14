using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand(Guid UserId, Guid ProductId, int Quantity) : IRequest<Result<bool>>;