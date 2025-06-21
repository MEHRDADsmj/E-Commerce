using MediatR;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(Guid UserId, string Token) : IRequest<Result<Guid>>;