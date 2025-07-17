namespace BlazorApp1.Domain.Entities;

public class Cart
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<CartItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    // Calculated properties
    public int TotalItems => Items.Sum(item => item.Quantity);
    public decimal Subtotal => Items.Sum(item => item.Subtotal);
    public decimal DeliveryFee => Subtotal > 50 ? 0 : 5.99m; // Free delivery over $50
    public decimal Tax => Subtotal * 0.08m; // 8% tax
    public decimal Total => Subtotal + DeliveryFee + Tax;
    public int EstimatedPrepTime => Items.Any() ? Items.Max(item => item.EstimatedPrepTime) : 0;
    
    public bool IsEmpty => !Items.Any();
}