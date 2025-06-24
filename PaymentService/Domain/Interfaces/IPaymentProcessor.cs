using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Interfaces;

public interface IPaymentProcessor
{
    Task<Payment> ProcessAsync(Guid orderId, Guid userId, decimal totalPrice);
}