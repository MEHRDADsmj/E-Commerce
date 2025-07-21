using OrderService.Domain.Entities;

namespace OrderService.Contracts.DTOs;

public class OrderDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public decimal TotalPrice { get; init; }
    public OrderStatus Status { get; init; }
    public IEnumerable<OrderItemDto> Items { get; init; }
}