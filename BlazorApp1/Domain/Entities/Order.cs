using BlazorApp1.Domain.Enums;

namespace BlazorApp1.Domain.Entities;

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? CustomerId { get; set; } // Null for guest orders
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public Address DeliveryAddress { get; set; } = new();
    public List<OrderItem> Items { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeliveryTime { get; set; }
    public DateTime? EstimatedDeliveryTime { get; set; }
    public string? Notes { get; set; }
    
    // Backward compatibility property
    public decimal TotalAmount { get; set; }
}

public class OrderItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => Price * Quantity;
}