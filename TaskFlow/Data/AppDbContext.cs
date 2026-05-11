using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

namespace TaskFlow.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<ProjectTask> ProjectTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<Project>()
            .HasOne(project => project.Owner)
            .WithMany(user => user.Projects)
            .HasForeignKey(project => project.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(task => task.Project)
            .WithMany(project => project.Tasks)
            .HasForeignKey(task => task.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(task => task.AssignedUser)
            .WithMany(user => user.AssignedTasks)
            .HasForeignKey(task => task.AssignedUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}