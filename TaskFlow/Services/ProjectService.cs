using TaskFlow.DTOs.Projects;
using TaskFlow.Interfaces.Repositories;
using TaskFlow.Interfaces.Services;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    
    public  ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectResponse>> GetAllAsync(Guid ownerId)
    {
        var project = await _projectRepository.GetAllByOwnerIdAsync(ownerId);

        return project
            .Select(MapToProjectResponse)
            .ToList();
    }

    public async Task<ProjectResponse> GetByIdAsync(int projectId, Guid ownerId)
    {
        var project = await _projectRepository.GetByIdAndOwnerIdAsync(projectId, ownerId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found");
        }
        
        return MapToProjectResponse(project);
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, Guid ownerId)
    {

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Project name is required");
        }
        
        var project = new Project
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow
        };
        
        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();
        
        return MapToProjectResponse(project);
    }

    public async Task<ProjectResponse> UpdateAsync(int projectId, UpdateProjectRequest request, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Project name is required");
        }
        
        var project = await _projectRepository.GetByIdAndOwnerIdAsync(projectId, ownerId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found");
        }
        
        project.Name = request.Name.Trim();
        project.Description = request.Description?.Trim();
        
        _projectRepository.Update(project);
        await _projectRepository.SaveChangesAsync();
        
        return MapToProjectResponse(project);
    }

    public async Task DeleteAsync(int projectId, Guid ownerId)
    {
        var project = await _projectRepository.GetByIdAndOwnerIdAsync(projectId, ownerId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found");
        }
        
        _projectRepository.Delete(project);
        await _projectRepository.SaveChangesAsync();
    }

    private static ProjectResponse MapToProjectResponse(Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            OwnerId = project.OwnerId,
            TaskCount = project.Tasks.Count
        };
    }
}