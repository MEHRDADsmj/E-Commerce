using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using StackExchange.Redis;

namespace CartService.Infrastructure.Repositories;

public class RedisCartRepository : ICartRepository
{
    private readonly IDatabase _database;

    public RedisCartRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }
    
    private string GetKey(Guid userId) => $"cart:{userId}";
    
    public async Task<Cart> GetAsync(Guid userId)
    {
        var data = await _database.StringGetAsync(GetKey(userId));
        if (data.IsNullOrEmpty) return Cart.Empty();
        
        return JsonSerializer.Deserialize<Cart>(data!) ?? Cart.Empty();
    }

    public async Task AddItemAsync(Guid userId, Guid productId, int quantity)
    {
        var cart = await GetAsync(userId);
        if (cart.IsEmpty()) return;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        if (item != null)
        {
            throw new DuplicateNameException("Product already exists");
        }

        item = new CartItem()
               {
                   ProductId = productId,
                   Quantity = quantity,
               };
        var context = new ValidationContext(item);
        Validator.ValidateObject(item, context, true);
        cart.Items.Add(item);
        
        await SaveAsync(cart);
    }

    public async Task RemoveItemAsync(Guid userId, Guid productId)
    {
        var cart = await GetAsync(userId);
        if (cart.IsEmpty()) return;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        if (item == null) return;
        cart.Items.Remove(item);
        await SaveAsync(cart);
    }

    public async Task UpdateItemQuantityAsync(Guid userId, Guid productId, int newQuantity)
    {
        var cart = await GetAsync(userId);
        if (cart.IsEmpty()) return;
        
        var item = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        if (item == null) return;
        item.Quantity = newQuantity;
        var context = new ValidationContext(item);
        Validator.ValidateObject(item, context, true);
        await SaveAsync(cart);
    }

    public async Task ClearCartAsync(Guid userId)
    {
        var cart = await GetAsync(userId);
        if (cart.IsEmpty()) return;
        
        cart.Items.Clear();
        await SaveAsync(cart);
    }

    public async Task<Cart> CreateCartAsync(Guid userId)
    {
        var cart = await GetAsync(userId);
        if (!cart.IsEmpty()) return cart;

        var newCart = new Cart(userId, new List<CartItem>());
        await SaveAsync(newCart);
        return newCart;
    }

    public async Task SaveAsync(Cart cart)
    {
        var data = JsonSerializer.Serialize(cart);
        await _database.StringSetAsync(GetKey(cart.UserId), data);
    }
}