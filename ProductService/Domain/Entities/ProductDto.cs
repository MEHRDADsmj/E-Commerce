namespace ProductService.Domain.Entities;

public class ProductDto
{
    public Guid Id { get; }
    public string? Name { get; }
    public string? Description { get; } 
    public decimal? UnitPrice { get; }

    public ProductDto(Guid id, string? name, decimal? unitPrice, string? description)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        Description = description;
    }
}