using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs.Projects;

public class UpdateProjectRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
}