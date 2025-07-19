using OrderService.Contracts.Entities;

namespace OrderService.Application.Interfaces;

public interface ICartClient
{
    Task<Cart> GetCartAsync(string token);
}