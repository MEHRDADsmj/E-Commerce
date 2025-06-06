using MediatR;
using OrderService.Domain.Entities;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<Order>>;