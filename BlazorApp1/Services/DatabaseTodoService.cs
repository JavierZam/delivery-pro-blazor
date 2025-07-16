// Service yang akses database langsung (Blazor Server only)
using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class DatabaseTodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;

        public DatabaseTodoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            // Direct database access - NO API!
            return await _context.Todos
                .OrderBy(t => t.IsCompleted)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<TodoItem> CreateAsync(TodoItem todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<TodoItem?> UpdateAsync(TodoItem todo)
        {
            var existingTodo = await _context.Todos.FindAsync(todo.Id);
            if (existingTodo == null) return null;

            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            
            await _context.SaveChangesAsync();
            return existingTodo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return false;

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TodoItem?> ToggleCompleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return null;

            todo.IsCompleted = !todo.IsCompleted;
            todo.CompletedAt = todo.IsCompleted ? DateTime.UtcNow : null;
            
            await _context.SaveChangesAsync();
            return todo;
        }
    }
}