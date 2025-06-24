using MediatR;
using Shared.Data;

namespace PaymentService.Application.Payments.Commands.HandleOrderCreated;

public record HandleOrderCreatedCommand(Guid OrderId, Guid UserId, decimal TotalPrice) : IRequest<Result<Unit>>;