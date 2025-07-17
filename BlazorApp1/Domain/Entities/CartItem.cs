namespace BlazorApp1.Domain.Entities;

public class CartItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public int EstimatedPrepTime { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.Now;
    
    public decimal Subtotal => Price * Quantity;
}