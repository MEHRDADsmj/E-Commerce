using CartService.Domain.Entities;
using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.GetCart;

public record GetCartQuery(Guid UserId) : IRequest<Result<Cart>>;