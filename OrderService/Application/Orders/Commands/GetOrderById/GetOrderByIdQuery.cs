using MediatR;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Entities;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderDto>>;