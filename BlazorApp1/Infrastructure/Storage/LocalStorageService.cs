using Blazored.LocalStorage;

namespace BlazorApp1.Infrastructure.Storage;

public class LocalStorageService : ILocalStorageService
{
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;

    public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        return await _localStorage.GetItemAsync<T>(key);
    }

    public async Task SetItemAsync<T>(string key, T item)
    {
        await _localStorage.SetItemAsync(key, item);
    }

    public async Task RemoveItemAsync(string key)
    {
        await _localStorage.RemoveItemAsync(key);
    }

    public async Task ClearAsync()
    {
        await _localStorage.ClearAsync();
    }
}