using System.ComponentModel.DataAnnotations;
using TaskFlow.Models;

namespace TaskFlow.DTOs.Tasks;

public class UpdateTaskStatusRequest
{
    [Required]
    public TaskItemStatus Status { get; set; }
}