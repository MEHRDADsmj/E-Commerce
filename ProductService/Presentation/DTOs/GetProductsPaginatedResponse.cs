using ProductService.Domain.Entities;

namespace ProductService.Presentation.DTOs;

public record GetProductsPaginatedResponseDto(IEnumerable<Product>? Products);