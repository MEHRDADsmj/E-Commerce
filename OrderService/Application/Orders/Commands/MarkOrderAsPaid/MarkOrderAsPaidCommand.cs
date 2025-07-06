using MediatR;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.MarkOrderAsPaid;

public record MarkOrderAsPaidCommand(Guid OrderId) : IRequest<Result<bool>>;