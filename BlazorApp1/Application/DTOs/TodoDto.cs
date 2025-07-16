// Application Layer - DTOs
namespace BlazorApp1.Application.DTOs
{
    public record TodoDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public bool IsCompleted { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? CompletedAt { get; init; }
    }

    public record CreateTodoDto
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }

    public record UpdateTodoDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
}