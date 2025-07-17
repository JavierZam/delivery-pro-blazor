namespace BlazorApp1.Infrastructure.Storage;

public interface ILocalStorageService
{
    Task<T?> GetItemAsync<T>(string key);
    Task SetItemAsync<T>(string key, T item);
    Task RemoveItemAsync(string key);
    Task ClearAsync();
}