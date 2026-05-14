using TaskFlow.Models;

namespace TaskFlow.DTOs.Tasks;

public class TaskResponse
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } =  string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public TaskItemStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? AssignedUserId { get; set; }
    public string? AssignedUserEmail { get; set; }
}