using OrderService.Application.Interfaces;
using OrderService.Contracts.Entities;

namespace OrderService.Infrastructure.ServiceClients;

public class HttpProductClient : IProductClient
{
    public Task<IEnumerable<ProductInfo>> GetProducts(List<Guid> productIds)
    {
        throw new NotImplementedException();
    }
}