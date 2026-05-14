using TaskFlow.DTOs.Projects;

namespace TaskFlow.Interfaces.Services;

public interface IProjectService 
{
    Task<List<ProjectResponse>> GetAllAsync(Guid ownerId);
    Task<ProjectResponse> GetByIdAsync(int projectId, Guid ownerId);
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request, Guid ownerId);
    Task<ProjectResponse> UpdateAsync(int projectId, UpdateProjectRequest request, Guid ownerId);
    Task DeleteAsync(int projectId, Guid ownerId);
}