using OrderService.Application.Interfaces;
using OrderService.Contracts.Entities;

namespace OrderService.Infrastructure.ServiceClients;

public class HttpCartClient : ICartClient
{
    public Task<Cart?> GetCartAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}