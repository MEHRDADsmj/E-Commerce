using MediatR;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.MarkOrderAsPaid;

public record MarkOrderAsPaidCommand(Guid Id) : IRequest<Result<bool>>;