using BlazorApp1.Domain.Entities;
using BlazorApp1.Infrastructure.Storage;

namespace BlazorApp1.Application.Services;

public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;
    private const string CART_KEY = "delivery_cart";
    private Cart? _cart;

    public event Action? OnCartChanged;

    public CartService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<Cart> GetCartAsync()
    {
        if (_cart == null)
        {
            _cart = await _localStorage.GetItemAsync<Cart>(CART_KEY) ?? new Cart();
        }
        return _cart;
    }

    public async Task AddItemAsync(Product product, int quantity = 1)
    {
        var cart = await GetCartAsync();
        var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            existingItem.AddedAt = DateTime.Now;
        }
        else
        {
            var cartItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductImage = product.ImageUrl,
                Price = product.Price,
                Quantity = quantity,
                Category = product.Category,
                EstimatedPrepTime = product.EstimatedPrepTime
            };
            cart.Items.Add(cartItem);
        }

        cart.UpdatedAt = DateTime.Now;
        await SaveCartAsync(cart);
    }

    public async Task RemoveItemAsync(string productId)
    {
        var cart = await GetCartAsync();
        var item = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        
        if (item != null)
        {
            cart.Items.Remove(item);
            cart.UpdatedAt = DateTime.Now;
            await SaveCartAsync(cart);
        }
    }

    public async Task UpdateQuantityAsync(string productId, int quantity)
    {
        if (quantity <= 0)
        {
            await RemoveItemAsync(productId);
            return;
        }

        var cart = await GetCartAsync();
        var item = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        
        if (item != null)
        {
            item.Quantity = quantity;
            cart.UpdatedAt = DateTime.Now;
            await SaveCartAsync(cart);
        }
    }

    public async Task ClearCartAsync()
    {
        var cart = new Cart();
        await SaveCartAsync(cart);
    }

    public async Task<int> GetCartItemCountAsync()
    {
        var cart = await GetCartAsync();
        return cart.TotalItems;
    }

    public async Task<CartItem?> GetCartItemAsync(string productId)
    {
        var cart = await GetCartAsync();
        return cart.Items.FirstOrDefault(item => item.ProductId == productId);
    }

    private async Task SaveCartAsync(Cart cart)
    {
        _cart = cart;
        await _localStorage.SetItemAsync(CART_KEY, cart);
        OnCartChanged?.Invoke();
    }
}