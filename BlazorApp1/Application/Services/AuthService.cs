using BlazorApp1.Domain.Entities;
using BlazorApp1.Infrastructure.Storage;
using System.Security.Cryptography;
using System.Text;

namespace BlazorApp1.Application.Services;

public class AuthService : IAuthService
{
    private readonly ILocalStorageService _localStorage;
    private const string USERS_KEY = "delivery_users";
    private const string SESSION_KEY = "delivery_session";
    private UserSession? _currentSession;

    public event Action<User?>? OnAuthStateChanged;

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        // Get all users
        var users = await GetUsersAsync();
        var user = users.FirstOrDefault(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));
        
        if (user == null)
        {
            return AuthResult.Failed("Email not found");
        }

        // Verify password
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            return AuthResult.Failed("Invalid password");
        }

        // Update last login
        user.LastLoginAt = DateTime.Now;
        await SaveUsersAsync(users);

        // Create session
        var token = GenerateToken();
        var session = new UserSession
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Token = token,
            ExpiresAt = request.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(8),
            RememberMe = request.RememberMe
        };

        await SaveSessionAsync(session);
        _currentSession = session;
        
        OnAuthStateChanged?.Invoke(user);
        return AuthResult.Succeeded(user, token);
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Email))
            return AuthResult.Failed("Email is required");
        
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            return AuthResult.Failed("Password must be at least 6 characters");
        
        if (request.Password != request.ConfirmPassword)
            return AuthResult.Failed("Passwords do not match");
        
        if (!request.AcceptTerms)
            return AuthResult.Failed("You must accept the terms and conditions");

        // Check if user already exists
        var users = await GetUsersAsync();
        if (users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return AuthResult.Failed("Email already registered");
        }

        // Create user
        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            PasswordHash = HashPassword(request.Password),
            IsEmailVerified = true // For demo purposes
        };

        users.Add(user);
        await SaveUsersAsync(users);

        // Auto login after registration
        var loginRequest = new LoginRequest
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = false
        };

        return await LoginAsync(loginRequest);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(SESSION_KEY);
        _currentSession = null;
        OnAuthStateChanged?.Invoke(null);
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var session = await GetCurrentSessionAsync();
        if (session == null || session.IsExpired)
        {
            return null;
        }

        var users = await GetUsersAsync();
        return users.FirstOrDefault(u => u.Id == session.UserId);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetCurrentUserAsync();
        return user != null;
    }

    public async Task<UserSession?> GetCurrentSessionAsync()
    {
        if (_currentSession != null && !_currentSession.IsExpired)
        {
            return _currentSession;
        }

        _currentSession = await _localStorage.GetItemAsync<UserSession>(SESSION_KEY);
        
        if (_currentSession?.IsExpired == true)
        {
            await LogoutAsync();
            return null;
        }

        return _currentSession;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var session = await GetCurrentSessionAsync();
        return session?.Token == token && !session.IsExpired;
    }

    public async Task<AuthResult> UpdateProfileAsync(User user)
    {
        var users = await GetUsersAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        
        if (existingUser == null)
        {
            return AuthResult.Failed("User not found");
        }

        // Update user properties (excluding password)
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Phone = user.Phone;
        existingUser.ProfilePictureUrl = user.ProfilePictureUrl;

        await SaveUsersAsync(users);
        OnAuthStateChanged?.Invoke(existingUser);
        
        return AuthResult.Succeeded(existingUser, _currentSession?.Token ?? "");
    }

    public async Task<AuthResult> ChangePasswordAsync(string oldPassword, string newPassword)
    {
        var user = await GetCurrentUserAsync();
        if (user == null)
        {
            return AuthResult.Failed("User not authenticated");
        }

        if (!VerifyPassword(oldPassword, user.PasswordHash))
        {
            return AuthResult.Failed("Current password is incorrect");
        }

        if (newPassword.Length < 6)
        {
            return AuthResult.Failed("New password must be at least 6 characters");
        }

        var users = await GetUsersAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.PasswordHash = HashPassword(newPassword);
            await SaveUsersAsync(users);
        }

        return AuthResult.Succeeded(user, _currentSession?.Token ?? "");
    }

    public async Task<AuthResult> AddAddressAsync(Address address)
    {
        var user = await GetCurrentUserAsync();
        if (user == null)
        {
            return AuthResult.Failed("User not authenticated");
        }

        var users = await GetUsersAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            // If this is the first address, make it default
            if (!existingUser.Addresses.Any())
            {
                address.IsDefault = true;
                existingUser.DefaultAddressId = address.Id;
            }

            existingUser.Addresses.Add(address);
            await SaveUsersAsync(users);
        }

        return AuthResult.Succeeded(user, _currentSession?.Token ?? "");
    }

    public async Task<AuthResult> UpdateAddressAsync(Address address)
    {
        var user = await GetCurrentUserAsync();
        if (user == null)
        {
            return AuthResult.Failed("User not authenticated");
        }

        var users = await GetUsersAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            var existingAddress = existingUser.Addresses.FirstOrDefault(a => a.Id == address.Id);
            if (existingAddress != null)
            {
                var index = existingUser.Addresses.IndexOf(existingAddress);
                existingUser.Addresses[index] = address;
                await SaveUsersAsync(users);
            }
        }

        return AuthResult.Succeeded(user, _currentSession?.Token ?? "");
    }

    public async Task<AuthResult> DeleteAddressAsync(string addressId)
    {
        var user = await GetCurrentUserAsync();
        if (user == null)
        {
            return AuthResult.Failed("User not authenticated");
        }

        var users = await GetUsersAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            var address = existingUser.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (address != null)
            {
                existingUser.Addresses.Remove(address);
                
                // If this was the default address, set another as default
                if (existingUser.DefaultAddressId == addressId && existingUser.Addresses.Any())
                {
                    var newDefault = existingUser.Addresses.First();
                    newDefault.IsDefault = true;
                    existingUser.DefaultAddressId = newDefault.Id;
                }
                else if (!existingUser.Addresses.Any())
                {
                    existingUser.DefaultAddressId = null;
                }

                await SaveUsersAsync(users);
            }
        }

        return AuthResult.Succeeded(user, _currentSession?.Token ?? "");
    }

    public async Task<AuthResult> SetDefaultAddressAsync(string addressId)
    {
        var user = await GetCurrentUserAsync();
        if (user == null)
        {
            return AuthResult.Failed("User not authenticated");
        }

        var users = await GetUsersAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            // Reset all addresses
            foreach (var addr in existingUser.Addresses)
            {
                addr.IsDefault = false;
            }

            // Set new default
            var address = existingUser.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (address != null)
            {
                address.IsDefault = true;
                existingUser.DefaultAddressId = addressId;
                await SaveUsersAsync(users);
            }
        }

        return AuthResult.Succeeded(user, _currentSession?.Token ?? "");
    }

    // Private helper methods
    private async Task<List<User>> GetUsersAsync()
    {
        return await _localStorage.GetItemAsync<List<User>>(USERS_KEY) ?? new List<User>();
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        await _localStorage.SetItemAsync(USERS_KEY, users);
    }

    private async Task SaveSessionAsync(UserSession session)
    {
        await _localStorage.SetItemAsync(SESSION_KEY, session);
    }

    private string HashPassword(string password)
    {
        // Simple hash for demo purposes - in production use proper hashing like BCrypt
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "DeliveryProSalt"));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }

    private string GenerateToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + 
               Convert.ToBase64String(BitConverter.GetBytes(DateTime.UtcNow.Ticks));
    }
}