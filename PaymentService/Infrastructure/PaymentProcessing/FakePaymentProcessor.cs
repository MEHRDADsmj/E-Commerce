using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Infrastructure.PaymentProcessing;

public class FakePaymentProcessor : IPaymentProcessor
{
    public async Task<Payment> ProcessAsync(Guid orderId, Guid userId, decimal totalPrice)
    {
        await Task.Delay(3000);
        var payment = new Payment(orderId, userId, totalPrice);
        payment.MarkAsCompleted();
        return payment;
    }
}