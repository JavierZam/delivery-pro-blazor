using BlazorApp1.Domain.Entities;
using BlazorApp1.Infrastructure.Storage;

namespace BlazorApp1.Application.Services;

public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;
    private readonly IAuthService _authService;
    private const string CART_KEY = "delivery_cart";
    private const string USER_CART_KEY_PREFIX = "delivery_cart_user_";
    private Cart? _cart;

    public event Action? OnCartChanged;

    public CartService(ILocalStorageService localStorage, IAuthService authService)
    {
        _localStorage = localStorage;
        _authService = authService;
    }

    public async Task<Cart> GetCartAsync()
    {
        if (_cart == null)
        {
            var cartKey = await GetCartKeyAsync();
            _cart = await _localStorage.GetItemAsync<Cart>(cartKey) ?? new Cart();
            
            // If user is authenticated and cart is empty, try to merge guest cart
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null && _cart.Items.Count == 0)
            {
                await MergeGuestCartAsync();
            }
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
        var cartKey = await GetCartKeyAsync();
        await _localStorage.SetItemAsync(cartKey, cart);
        OnCartChanged?.Invoke();
    }

    private async Task<string> GetCartKeyAsync()
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        return currentUser != null ? $"{USER_CART_KEY_PREFIX}{currentUser.Id}" : CART_KEY;
    }

    private async Task MergeGuestCartAsync()
    {
        try
        {
            // Get guest cart
            var guestCart = await _localStorage.GetItemAsync<Cart>(CART_KEY);
            if (guestCart?.Items?.Any() == true)
            {
                // Merge guest cart items into user cart
                foreach (var guestItem in guestCart.Items)
                {
                    var existingItem = _cart!.Items.FirstOrDefault(item => item.ProductId == guestItem.ProductId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += guestItem.Quantity;
                        existingItem.AddedAt = DateTime.Now;
                    }
                    else
                    {
                        _cart.Items.Add(guestItem);
                    }
                }

                _cart.UpdatedAt = DateTime.Now;
                
                // Save merged cart and clear guest cart
                await SaveCartAsync(_cart);
                await _localStorage.RemoveItemAsync(CART_KEY);
            }
        }
        catch (Exception)
        {
            // Ignore merge errors to prevent disrupting user experience
        }
    }

    public async Task TransferGuestCartToUserAsync()
    {
        // Force merge when user logs in
        _cart = null; // Clear cached cart
        await GetCartAsync(); // This will trigger merge
    }

    public async Task ClearUserCartCacheAsync()
    {
        // Clear cached cart when user logs out
        _cart = null;
    }
}