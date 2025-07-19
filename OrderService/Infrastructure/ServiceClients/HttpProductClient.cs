using System.Net.Http.Headers;
using System.Text.Json;
using OrderService.Application.Interfaces;
using OrderService.Contracts.DTOs;
using OrderService.Contracts.Entities;

namespace OrderService.Infrastructure.ServiceClients;

public class HttpProductClient : IProductClient
{
    private readonly HttpClient _httpClient;

    public HttpProductClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<ProductInfo>> GetProducts(IEnumerable<Guid> productIds, string token)
    {
        var opt = new JsonSerializerOptions
                  {
                      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                  };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var products = new ProductIdsDto(productIds);
        var response = await _httpClient.PostAsJsonAsync("products/bulk", products, opt);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ProductInfo>>(opt) ?? Array.Empty<ProductInfo>();
    }
}