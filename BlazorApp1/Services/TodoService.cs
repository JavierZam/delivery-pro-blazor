using BlazorApp1.Models;

namespace BlazorApp1.Services
{
    public interface ITodoService
    {
        Task<List<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem> CreateAsync(TodoItem todo);
        Task<TodoItem?> UpdateAsync(TodoItem todo);
        Task<bool> DeleteAsync(int id);
        Task<TodoItem?> ToggleCompleteAsync(int id);
    }

    public class TodoService : ITodoService
    {
        private readonly List<TodoItem> _todos = new();
        private int _nextId = 1;

        public TodoService()
        {
            // Seed data untuk demo
            _todos.AddRange(new[]
            {
                new TodoItem { Id = _nextId++, Title = "Learn Blazor", Description = "Understand Blazor fundamentals" },
                new TodoItem { Id = _nextId++, Title = "Build Todo App", Description = "Create a todo application", IsCompleted = true, CompletedAt = DateTime.Now.AddHours(-1) }
            });
        }

        public Task<List<TodoItem>> GetAllAsync()
        {
            return Task.FromResult(_todos.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt).ToList());
        }

        public Task<TodoItem?> GetByIdAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(todo);
        }

        public Task<TodoItem> CreateAsync(TodoItem todo)
        {
            todo.Id = _nextId++;
            todo.CreatedAt = DateTime.Now;
            _todos.Add(todo);
            return Task.FromResult(todo);
        }

        public Task<TodoItem?> UpdateAsync(TodoItem todo)
        {
            var existingTodo = _todos.FirstOrDefault(t => t.Id == todo.Id);
            if (existingTodo == null) return Task.FromResult<TodoItem?>(null);

            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            return Task.FromResult<TodoItem?>(existingTodo);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return Task.FromResult(false);

            _todos.Remove(todo);
            return Task.FromResult(true);
        }

        public Task<TodoItem?> ToggleCompleteAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return Task.FromResult<TodoItem?>(null);

            todo.IsCompleted = !todo.IsCompleted;
            todo.CompletedAt = todo.IsCompleted ? DateTime.Now : null;
            return Task.FromResult<TodoItem?>(todo);
        }
    }
}