using BlazorApp1.Domain.Entities;

namespace BlazorApp1.Application.Services;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync();
    Task<List<Product>> GetProductsByCategoryAsync(string category);
    Task<Product?> GetProductByIdAsync(string id);
}