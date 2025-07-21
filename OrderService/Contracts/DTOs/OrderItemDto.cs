namespace OrderService.Contracts.DTOs;

public class OrderItemDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public string ProductName { get; init; }
    public decimal UnitPrice { get; init; }
}