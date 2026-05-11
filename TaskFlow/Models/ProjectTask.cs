namespace TaskFlow.Models;

public class ProjectTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public int? AssignedUserId { get; set; }
    public AppUser?  AssignedUser { get; set; }
}