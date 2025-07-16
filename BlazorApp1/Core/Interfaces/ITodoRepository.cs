// Domain Layer - Repository Interface
using BlazorApp1.Core.Entities;

namespace BlazorApp1.Core.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo?> GetByIdAsync(int id);
        Task<Todo> AddAsync(Todo todo);
        Task<Todo> UpdateAsync(Todo todo);
        Task<bool> DeleteAsync(int id);
    }
}