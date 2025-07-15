using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Entities;

public class Product
{
    [Key] public Guid Id { get; private init; }
    [Required, StringLength(64)] public string Name { get; private set; }
    [StringLength(256)] public string? Description { get; private set; }
    [Required, Range(0, double.MaxValue)] public decimal UnitPrice { get; private set; }

    public static Product Empty()
    {
        return new Product("Null", 0, "Null");
    }

    public Product() : this("Null", 0, "Null")
    {
        
    }
    
    public Product(string name, decimal unitPrice, string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        UnitPrice = unitPrice;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }

    public void Update(Product product)
    {
        Name = product.Name;
        Description = product.Description;
        UnitPrice = product.UnitPrice;
    }
}