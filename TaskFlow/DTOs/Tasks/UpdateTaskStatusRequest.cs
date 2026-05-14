using TaskFlow.Models;

namespace TaskFlow.DTOs.Tasks;

public class UpdateTaskStatusRequest
{
    public TaskItemStatus Status { get; set; }
}