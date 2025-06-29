using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Contracts.Events;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using Shared.Data;

namespace PaymentService.Application.Payments.Commands.HandleOrderCreated;

public class HandleOrderCreatedHandler : IRequestHandler<HandleOrderCreatedCommand, Result<Unit>>
{
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IEventPublisher _eventPublisher;

    public HandleOrderCreatedHandler(IPaymentProcessor paymentProcessor, IEventPublisher eventPublisher)
    {
        _paymentProcessor = paymentProcessor;
        _eventPublisher = eventPublisher;
    }


    public async Task<Result<Unit>> Handle(HandleOrderCreatedCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentProcessor.ProcessAsync(request.OrderId, request.UserId, request.TotalPrice);

        if (payment.Status == PaymentStatus.Completed)
        {
            await _eventPublisher.PublishAsync(new PaymentCompletedEvent(payment.OrderId), "payment_completed");
            return Result<Unit>.Success(Unit.Value);
        }
        else
        {
            await _eventPublisher.PublishAsync(new PaymentFailedEvent(payment.OrderId), "payment_failed");
            return Result<Unit>.Failure("Payment failed");
        }
    }
}