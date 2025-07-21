using OrderService.Contracts.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Presentation.DTOs;

public record GetOrderByUserIdResponseDto(IEnumerable<OrderDto> Orders);