using BlazorApp1.Domain.Entities;

namespace BlazorApp1.Application.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products;

    public ProductService()
    {
        _products = GetSampleProducts();
    }

    public Task<List<Product>> GetProductsAsync()
    {
        return Task.FromResult(_products);
    }

    public Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        var filteredProducts = _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        return Task.FromResult(filteredProducts);
    }

    public Task<Product?> GetProductByIdAsync(string id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    private List<Product> GetSampleProducts()
    {
        return new List<Product>
        {
            // Pizza
            new() { Id = "1", Name = "Margherita Pizza", Description = "Classic pizza with tomato sauce, mozzarella, and basil", Price = 14.99m, Category = "Pizza", ImageUrl = "/images/margherita.jpg", EstimatedPrepTime = 15 },
            new() { Id = "2", Name = "Pepperoni Pizza", Description = "Traditional pizza with pepperoni and mozzarella cheese", Price = 16.99m, Category = "Pizza", ImageUrl = "/images/pepperoni.jpg", EstimatedPrepTime = 15 },
            new() { Id = "3", Name = "Supreme Pizza", Description = "Loaded with pepperoni, sausage, peppers, onions, and mushrooms", Price = 19.99m, Category = "Pizza", ImageUrl = "/images/supreme.jpg", EstimatedPrepTime = 18 },
            
            // Burgers
            new() { Id = "4", Name = "Classic Burger", Description = "Beef patty with lettuce, tomato, onion, and special sauce", Price = 12.99m, Category = "Burgers", ImageUrl = "/images/classic-burger.jpg", EstimatedPrepTime = 12 },
            new() { Id = "5", Name = "Chicken Burger", Description = "Grilled chicken breast with avocado and chipotle mayo", Price = 13.99m, Category = "Burgers", ImageUrl = "/images/chicken-burger.jpg", EstimatedPrepTime = 12 },
            new() { Id = "6", Name = "Veggie Burger", Description = "Plant-based patty with fresh vegetables and vegan mayo", Price = 11.99m, Category = "Burgers", ImageUrl = "/images/veggie-burger.jpg", EstimatedPrepTime = 10 },
            
            // Sushi
            new() { Id = "7", Name = "California Roll", Description = "Crab, cucumber, and avocado with sesame seeds", Price = 8.99m, Category = "Sushi", ImageUrl = "/images/california-roll.jpg", EstimatedPrepTime = 8 },
            new() { Id = "8", Name = "Salmon Nigiri", Description = "Fresh salmon over seasoned sushi rice (2 pieces)", Price = 6.99m, Category = "Sushi", ImageUrl = "/images/salmon-nigiri.jpg", EstimatedPrepTime = 5 },
            new() { Id = "9", Name = "Rainbow Roll", Description = "California roll topped with assorted sashimi", Price = 15.99m, Category = "Sushi", ImageUrl = "/images/rainbow-roll.jpg", EstimatedPrepTime = 12 },
            
            // Chinese
            new() { Id = "10", Name = "Kung Pao Chicken", Description = "Spicy stir-fried chicken with peanuts and vegetables", Price = 13.99m, Category = "Chinese", ImageUrl = "/images/kung-pao.jpg", EstimatedPrepTime = 15 },
            new() { Id = "11", Name = "Sweet & Sour Pork", Description = "Crispy pork with bell peppers in sweet and sour sauce", Price = 14.99m, Category = "Chinese", ImageUrl = "/images/sweet-sour-pork.jpg", EstimatedPrepTime = 18 },
            new() { Id = "12", Name = "Vegetable Lo Mein", Description = "Soft noodles stir-fried with mixed vegetables", Price = 11.99m, Category = "Chinese", ImageUrl = "/images/lo-mein.jpg", EstimatedPrepTime = 12 },
            
            // Indian
            new() { Id = "13", Name = "Chicken Tikka Masala", Description = "Grilled chicken in creamy tomato-based sauce", Price = 15.99m, Category = "Indian", ImageUrl = "/images/tikka-masala.jpg", EstimatedPrepTime = 20 },
            new() { Id = "14", Name = "Vegetable Biryani", Description = "Aromatic basmati rice with mixed vegetables and spices", Price = 12.99m, Category = "Indian", ImageUrl = "/images/biryani.jpg", EstimatedPrepTime = 25 },
            new() { Id = "15", Name = "Garlic Naan", Description = "Fresh baked bread with garlic and herbs", Price = 3.99m, Category = "Indian", ImageUrl = "/images/garlic-naan.jpg", EstimatedPrepTime = 8 },
            
            // Desserts
            new() { Id = "16", Name = "Chocolate Cake", Description = "Rich chocolate cake with chocolate frosting", Price = 6.99m, Category = "Desserts", ImageUrl = "/images/chocolate-cake.jpg", EstimatedPrepTime = 5 },
            new() { Id = "17", Name = "Cheesecake", Description = "New York style cheesecake with berry compote", Price = 7.99m, Category = "Desserts", ImageUrl = "/images/cheesecake.jpg", EstimatedPrepTime = 5 },
            new() { Id = "18", Name = "Ice Cream Sundae", Description = "Vanilla ice cream with chocolate sauce and whipped cream", Price = 5.99m, Category = "Desserts", ImageUrl = "/images/sundae.jpg", EstimatedPrepTime = 3 }
        };
    }
}