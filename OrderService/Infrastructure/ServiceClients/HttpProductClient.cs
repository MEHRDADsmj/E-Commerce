using System.Text.Json;
using OrderService.Application.Interfaces;
using OrderService.Contracts.Entities;

namespace OrderService.Infrastructure.ServiceClients;

public class HttpProductClient : IProductClient
{
    private readonly HttpClient _httpClient;

    public HttpProductClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    
    public async Task<IEnumerable<ProductInfo>> GetProducts(List<Guid> productIds)
    {
        var opt = new JsonSerializerOptions
                  {
                      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                  };
        var response = await _httpClient.PostAsJsonAsync($"api/products/bulk", productIds, opt);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ProductInfo>>() ?? Array.Empty<ProductInfo>();
    }
}