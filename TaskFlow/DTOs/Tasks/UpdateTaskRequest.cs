using System.ComponentModel.DataAnnotations;
using TaskFlow.Models;

namespace TaskFlow.DTOs.Tasks;

public class UpdateTaskRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public Guid? AssignedUserId { get; set; }
}