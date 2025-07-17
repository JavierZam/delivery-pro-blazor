namespace BlazorApp1.Domain.Entities;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool AcceptTerms { get; set; } = false;
}

public class AuthResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public User? User { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public static AuthResult Failed(params string[] errors)
    {
        return new AuthResult
        {
            Success = false,
            Errors = errors.ToList()
        };
    }
    
    public static AuthResult Succeeded(User user, string token)
    {
        return new AuthResult
        {
            Success = true,
            User = user,
            Token = token
        };
    }
}

public class UserSession
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime LoginTime { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
    public bool RememberMe { get; set; } = false;
    
    public bool IsExpired => DateTime.Now > ExpiresAt;
}