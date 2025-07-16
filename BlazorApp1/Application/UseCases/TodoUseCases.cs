// Application Layer - Use Cases
using BlazorApp1.Application.DTOs;
using BlazorApp1.Core.Entities;
using BlazorApp1.Core.Interfaces;

namespace BlazorApp1.Application.UseCases
{
    public interface ITodoUseCases
    {
        Task<IEnumerable<TodoDto>> GetAllTodosAsync();
        Task<TodoDto?> GetTodoByIdAsync(int id);
        Task<TodoDto> CreateTodoAsync(CreateTodoDto createDto);
        Task<TodoDto?> UpdateTodoAsync(UpdateTodoDto updateDto);
        Task<bool> DeleteTodoAsync(int id);
        Task<TodoDto?> ToggleTodoAsync(int id);
    }

    public class TodoUseCases : ITodoUseCases
    {
        private readonly ITodoRepository _repository;

        public TodoUseCases(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
        {
            var todos = await _repository.GetAllAsync();
            return todos.Select(MapToDto).OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt);
        }

        public async Task<TodoDto?> GetTodoByIdAsync(int id)
        {
            var todo = await _repository.GetByIdAsync(id);
            return todo == null ? null : MapToDto(todo);
        }

        public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createDto)
        {
            var todo = Todo.Create(createDto.Title, createDto.Description);
            var createdTodo = await _repository.AddAsync(todo);
            return MapToDto(createdTodo);
        }

        public async Task<TodoDto?> UpdateTodoAsync(UpdateTodoDto updateDto)
        {
            var todo = await _repository.GetByIdAsync(updateDto.Id);
            if (todo == null) return null;

            todo.Update(updateDto.Title, updateDto.Description);
            var updatedTodo = await _repository.UpdateAsync(todo);
            return MapToDto(updatedTodo);
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<TodoDto?> ToggleTodoAsync(int id)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo == null) return null;

            todo.ToggleStatus();
            var updatedTodo = await _repository.UpdateAsync(todo);
            return MapToDto(updatedTodo);
        }

        private static TodoDto MapToDto(Todo todo)
        {
            return new TodoDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                CompletedAt = todo.CompletedAt
            };
        }
    }
}