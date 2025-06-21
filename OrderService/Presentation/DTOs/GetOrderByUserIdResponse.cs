using OrderService.Domain.Entities;

namespace OrderService.Presentation.DTOs;

public record GetOrderByUserIdResponseDto(IEnumerable<Order>? Orders);