using BlazorApp1.Domain.Entities;

namespace BlazorApp1.Application.Services;

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order?> GetOrderByIdAsync(string id);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(string id);
    Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
}