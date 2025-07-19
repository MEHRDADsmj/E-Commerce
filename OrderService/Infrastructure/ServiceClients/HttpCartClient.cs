using System.ComponentModel.DataAnnotations;
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
    
    public async Task<Cart> GetCartAsync(string token)
    {
        var opt = new JsonSerializerOptions
                  {
                      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                  };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("carts");
        response.EnsureSuccessStatusCode();
        try
        {
            var dto = await response.Content.ReadFromJsonAsync<CartDto>(opt);
            return dto == null ? Cart.Empty() : dto.Cart;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e);
            return Cart.Empty();
        }
    }
}