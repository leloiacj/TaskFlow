using TaskFlow.DTOs.Tasks;

namespace TaskFlow.Interfaces.Services;

public interface ITaskService
{
    Task<List<TaskResponse>> GetAllAsync(Guid ownerId);

    Task<List<TaskResponse>> GetByProjectIdAsync(int projectId, Guid ownerId);

    Task<TaskResponse> GetByIdAsync(int taskId, Guid ownerId);

    Task<TaskResponse> CreateAsync(CreateTaskRequest request, Guid ownerId);

    Task<TaskResponse> UpdateAsync(int taskId, UpdateTaskRequest request, Guid ownerId);

    Task<TaskResponse> UpdateStatusAsync(int taskId, UpdateTaskStatusRequest request, Guid ownerId);

    Task DeleteAsync(int taskId, Guid ownerId);
}