using MediatR;
using OrderService.Contracts.DTOs;
using Shared.Data;

namespace OrderService.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderDto>>;