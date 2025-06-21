using System.Net.Http.Headers;
using System.Text.Json;
using OrderService.Application.Interfaces;
using OrderService.Contracts.DTOs;
using OrderService.Contracts.Entities;

namespace OrderService.Infrastructure.ServiceClients;

public class HttpCartClient : ICartClient
{
    private readonly HttpClient _httpClient;

    public HttpCartClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<Cart?> GetCartAsync(Guid userId, string token)
    {
        var opt = new JsonSerializerOptions
                  {
                      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                  };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("carts");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CartDto>(opt)).Cart ?? null;
    }
}