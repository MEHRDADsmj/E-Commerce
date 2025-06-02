using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Entities;

public class Product
{
    [Key] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    public string? Description { get; set; }
    [Required] public decimal UnitPrice { get; set; }

    public Product()
    {
        
    }
    
    public Product(string name, decimal unitPrice, string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        UnitPrice = unitPrice;
    }
}