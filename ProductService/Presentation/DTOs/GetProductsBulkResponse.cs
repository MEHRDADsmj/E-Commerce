using ProductService.Domain.Entities;

namespace ProductService.Presentation.DTOs;

public record GetProductsBulkResponseDto(IEnumerable<Product> Products);