using BlazorApp1.Domain.Entities;

namespace BlazorApp1.Application.Services;

public interface IAuthService
{
    event Action<User?>? OnAuthStateChanged;
    
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<User?> GetCurrentUserAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserSession?> GetCurrentSessionAsync();
    Task<bool> ValidateTokenAsync(string token);
    
    // User management
    Task<AuthResult> UpdateProfileAsync(User user);
    Task<AuthResult> ChangePasswordAsync(string oldPassword, string newPassword);
    Task<AuthResult> AddAddressAsync(Address address);
    Task<AuthResult> UpdateAddressAsync(Address address);
    Task<AuthResult> DeleteAddressAsync(string addressId);
    Task<AuthResult> SetDefaultAddressAsync(string addressId);
}