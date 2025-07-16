// Infrastructure Layer - Repository Implementation
using BlazorApp1.Core.Entities;
using BlazorApp1.Core.Interfaces;

namespace BlazorApp1.Infrastructure.Repositories
{
    public class InMemoryTodoRepository : ITodoRepository
    {
        private readonly List<Todo> _todos = new();
        private int _nextId = 1;

        public InMemoryTodoRepository()
        {
            // Seed data
            var todo1 = Todo.Create("Learn Clean Architecture", "Understand separation of concerns");
            var todo2 = Todo.Create("Implement Blazor App", "Build todo app with proper architecture");
            
            // Use reflection to set Id (simulating database auto-increment)
            SetId(todo1, _nextId++);
            SetId(todo2, _nextId++);
            
            todo2.Complete(); // Mark second todo as completed
            
            _todos.AddRange(new[] { todo1, todo2 });
        }

        public Task<IEnumerable<Todo>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Todo>>(_todos.ToList());
        }

        public Task<Todo?> GetByIdAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(todo);
        }

        public Task<Todo> AddAsync(Todo todo)
        {
            SetId(todo, _nextId++);
            _todos.Add(todo);
            return Task.FromResult(todo);
        }

        public Task<Todo> UpdateAsync(Todo todo)
        {
            var index = _todos.FindIndex(t => t.Id == todo.Id);
            if (index >= 0)
            {
                _todos[index] = todo;
            }
            return Task.FromResult(todo);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return Task.FromResult(false);

            _todos.Remove(todo);
            return Task.FromResult(true);
        }

        // Helper method to set Id using reflection (simulating database behavior)
        private static void SetId(Todo todo, int id)
        {
            var idProperty = typeof(Todo).GetProperty("Id");
            idProperty?.SetValue(todo, id);
        }
    }
}