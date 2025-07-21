using MediatR;
using OrderService.Contracts.DTOs;
using Shared.Data;

namespace OrderService.Application.Orders.Queries.GetOrderByUserId;

public record GetOrderByUserIdQuery(Guid UserId) : IRequest<Result<IEnumerable<OrderDto>>>;