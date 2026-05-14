using TaskFlow.Models;

namespace TaskFlow.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<List<ProjectTask>> GetAllByOwenerIdAsync(Guid ownerId);
    Task<List<ProjectTask>> GetAllByProjecIdAndOwnerIdAsync(int projectId, Guid ownerId);
    Task<ProjectTask?> GetByIdAndOwnerIdAsync(int taskId, Guid ownerId);
    Task AddAsync(ProjectTask task);
    void Update(ProjectTask task);
    void Delete(ProjectTask task);
    Task SaveChangesAsync();
}