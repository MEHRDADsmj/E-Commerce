using System.ComponentModel.DataAnnotations;

namespace PaymentService.Domain.Entities;

public class Payment
{
    [Key] public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public decimal TotalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public PaymentStatus Status { get; private set; }

    public Payment(Guid orderId, Guid userId, decimal totalPrice)
    {
        OrderId = orderId;
        UserId = userId;
        TotalPrice = totalPrice;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = PaymentStatus.Completed;
    }

    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;
    }
}