using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Entities;

public class ProductDto
{
    [Key] public Guid Id { get; }
    [Required, StringLength(64)] private string Name { get; }
    [StringLength(256)] private string? Description { get; }
    [Required, Range(0, double.MaxValue)] private decimal UnitPrice { get; }

    public ProductDto(Guid id, string name, decimal unitPrice, string? description)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        Description = description;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }

    public Product ToProduct()
    {
        return new Product(Name, UnitPrice, Description);
    }
}