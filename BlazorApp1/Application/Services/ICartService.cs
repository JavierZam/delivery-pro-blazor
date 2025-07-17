using BlazorApp1.Domain.Entities;

namespace BlazorApp1.Application.Services;

public interface ICartService
{
    event Action? OnCartChanged;
    
    Task<Cart> GetCartAsync();
    Task AddItemAsync(Product product, int quantity = 1);
    Task RemoveItemAsync(string productId);
    Task UpdateQuantityAsync(string productId, int quantity);
    Task ClearCartAsync();
    Task<int> GetCartItemCountAsync();
    Task<CartItem?> GetCartItemAsync(string productId);
    Task TransferGuestCartToUserAsync();
    Task ClearUserCartCacheAsync();
}