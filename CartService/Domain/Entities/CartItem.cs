﻿namespace CartService.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}