using TaskFlow.Models;

namespace TaskFlow.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllByOwnerIdAsync(Guid ownerId);
    Task<Project?> GetByIdAndOwnerIdAsync(int projectId, Guid ownerId);
    Task<Project?> GetByIdAsync(int projectId);
    Task AddAsync(Project project);
    void Update(Project project);
    void Delete(Project project);
    Task<bool> ExistsForOwnerAsync(int projectId, Guid ownerId);
    Task SaveChangesAsync();

}