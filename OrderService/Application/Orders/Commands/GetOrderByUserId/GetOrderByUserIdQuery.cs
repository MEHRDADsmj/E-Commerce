using MediatR;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Entities;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.GetOrderByUserId;

public record GetOrderByUserIdQuery(Guid UserId) : IRequest<Result<IEnumerable<OrderDto>>>;