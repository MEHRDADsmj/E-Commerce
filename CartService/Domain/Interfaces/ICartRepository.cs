using CartService.Domain.Entities;

namespace CartService.Domain.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetAsync(Guid userId);
    Task AddItemAsync(Guid userId, Guid productId, int quantity);
    Task RemoveItemAsync(Guid userId, Guid productId);
    Task UpdateItemQuantityAsync(Guid userId, Guid productId, int newQuantity);
    Task ClearCartAsync(Guid userId);
    Task<Cart?> CreateCartAsync(Guid userId);
    Task SaveAsync(Cart cart);
}