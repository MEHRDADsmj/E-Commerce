namespace ProductService.Domain.Entities;

public record ProductDto(Guid Id, string Name, decimal UnitPrice, string? Description);