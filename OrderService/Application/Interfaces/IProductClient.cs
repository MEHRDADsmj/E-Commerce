﻿using OrderService.Contracts.Entities;

namespace OrderService.Application.Interfaces;

public interface IProductClient
{
    Task<IEnumerable<ProductInfo>> GetProducts(IEnumerable<Guid> productIds, string token);
}