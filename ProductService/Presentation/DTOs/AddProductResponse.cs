namespace ProductService.Presentation.DTOs;

public record AddProductResponseDto(Guid Id, string Name, decimal UnitPrice);