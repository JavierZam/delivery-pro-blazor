namespace BlazorApp1.Domain.Entities;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public List<Address> Addresses { get; set; } = new();
    public string? DefaultAddressId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastLoginAt { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    public string? ProfilePictureUrl { get; set; }
    
    // Calculated properties
    public string FullName => $"{FirstName} {LastName}".Trim();
    public Address? DefaultAddress => Addresses.FirstOrDefault(a => a.Id == DefaultAddressId);
}

public class Address
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Label { get; set; } = string.Empty; // Home, Work, etc.
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string? Instructions { get; set; } // Delivery instructions
    public bool IsDefault { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}