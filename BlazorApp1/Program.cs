using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp1;
using BlazorApp1.Application.Services;
using BlazorApp1.Infrastructure.Storage;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<BlazorApp1.Components.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add LocalStorage for offline functionality
builder.Services.AddBlazoredLocalStorage();

// Register our custom services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<BlazorApp1.Infrastructure.Storage.ILocalStorageService>(provider => 
    new BlazorApp1.Infrastructure.Storage.LocalStorageService(provider.GetRequiredService<Blazored.LocalStorage.ILocalStorageService>()));

await builder.Build().RunAsync();
