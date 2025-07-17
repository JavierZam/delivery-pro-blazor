using BlazorApp1.Domain.Entities;
using BlazorApp1.Domain.Enums;
using BlazorApp1.Infrastructure.Storage;

namespace BlazorApp1.Application.Services;

public class OrderService : IOrderService
{
    private readonly ILocalStorageService _localStorage;
    private const string ORDERS_KEY = "delivery_orders";

    public OrderService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _localStorage.GetItemAsync<List<Order>>(ORDERS_KEY) ?? [];
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        var orders = await GetOrdersAsync();
        return orders.FirstOrDefault(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        var orders = await GetOrdersAsync();
        orders.Add(order);
        await _localStorage.SetItemAsync(ORDERS_KEY, orders);
        return order;
    }

    public async Task<Order?> UpdateOrderAsync(Order order)
    {
        var orders = await GetOrdersAsync();
        var index = orders.FindIndex(o => o.Id == order.Id);
        if (index >= 0)
        {
            orders[index] = order;
            await _localStorage.SetItemAsync(ORDERS_KEY, orders);
            return order;
        }
        return null;
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
        var orders = await GetOrdersAsync();
        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            orders.Remove(order);
            await _localStorage.SetItemAsync(ORDERS_KEY, orders);
            return true;
        }
        return false;
    }

    public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        var orders = await GetOrdersAsync();
        return orders.Where(o => o.Status == status).ToList();
    }
}