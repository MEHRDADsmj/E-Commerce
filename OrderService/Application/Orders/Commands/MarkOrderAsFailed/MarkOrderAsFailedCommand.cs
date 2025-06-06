using MediatR;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.MarkOrderAsFailed;

public record MarkOrderAsFailedCommand(Guid Id) : IRequest<Result<bool>>;