// Domain Layer - Core Entities
namespace BlazorApp1.Core.Entities
{
    public class Todo
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // Private constructor for Entity Framework or Serialization
        private Todo() { }

        // Factory method - Domain logic
        public static Todo Create(string title, string description = "")
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            return new Todo
            {
                Title = title,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = false
            };
        }

        // Domain methods
        public void Update(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            Title = title;
            Description = description;
        }

        public void Complete()
        {
            if (IsCompleted) return;
            
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }

        public void Reopen()
        {
            if (!IsCompleted) return;
            
            IsCompleted = false;
            CompletedAt = null;
        }

        public void ToggleStatus()
        {
            if (IsCompleted)
                Reopen();
            else
                Complete();
        }
    }
}