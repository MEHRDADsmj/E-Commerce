﻿using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Entities;

public class OrderItem
{
    [Key] public Guid Id { get; private set; }
    [Required] public Guid ProductId { get; private set; }
    [StringLength(64)] public string ProductName { get; private set; }
    [Required, Range(1, double.MaxValue)] public decimal UnitPrice { get; private set; }
    [Range(1, int.MaxValue)] public int Quantity { get; private set; }
    
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }
}