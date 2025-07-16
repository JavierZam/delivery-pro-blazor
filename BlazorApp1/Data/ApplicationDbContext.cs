// Blazor Server bisa langsung akses database
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Seed data
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem { Id = 1, Title = "Learn Blazor Server", Description = "Direct database access", CreatedAt = DateTime.UtcNow },
                new TodoItem { Id = 2, Title = "Build fullstack app", Description = "No API needed!", CreatedAt = DateTime.UtcNow }
            );
        }
    }
}