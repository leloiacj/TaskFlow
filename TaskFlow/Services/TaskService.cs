using TaskFlow.DTOs.Tasks;
using TaskFlow.Interfaces.Repositories;
using TaskFlow.Interfaces.Services;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class TaskService : ITaskService
{
    public readonly ITaskRepository _taskRepository;
    public readonly IProjectRepository _projectRepository;

    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<List<TaskResponse>> GetAllAsync(Guid ownerId)
    {
        var tasks = await _taskRepository.GetAllByOwenerIdAsync(ownerId);
        
        return tasks
            .Select(MapToTaskResponse)
            .ToList();
    }

    public async Task<List<TaskResponse>> GetByProjectIdAsync(int projectId, Guid ownerId)
    {
        var projectExists = await _projectRepository.ExistsForOwnerAsync(projectId, ownerId);

        if (!projectExists)
        {
            throw new KeyNotFoundException("Project not found");
        }
        
        var tasks = await _taskRepository.GetAllByProjecIdAndOwnerIdAsync(projectId, ownerId);
        
        return tasks
            .Select(MapToTaskResponse)
            .ToList();
    }

    public async Task<TaskResponse> GetByIdAsync(int taskId, Guid ownerId)
    {
        var tasks = await _taskRepository.GetByIdAndOwnerIdAsync(taskId, ownerId);

        if (tasks is null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        
        return MapToTaskResponse(tasks);
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest request, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title is required");
        }
        
        if (request.ProjectId <= 0)
        {
            throw new ArgumentException("ProjectId must be greater than zero");
        }
        
        var project = await _projectRepository.GetByIdAndOwnerIdAsync(request.ProjectId, ownerId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        if (request.AssignedUserId.HasValue && request.AssignedUserId.Value != ownerId)
        {
            throw new InvalidOperationException("You can only assign tasks to yourself in this version.");
        }

        var task = new ProjectTask
        {
            ProjectId = request.ProjectId,
            Project = project,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            Priority = request.Priority,
            Status = TaskItemStatus.ToDo,
            DueDate = request.DueDate,
            AssignedUserId = request.AssignedUserId,
            CreatedAt = DateTime.UtcNow
        };
        
        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();
        
        var createdTask = await _taskRepository.GetByIdAndOwnerIdAsync(task.Id, ownerId);

        if (createdTask is null)
        {
            throw new InvalidOperationException("Task was created but could not be loaded");
        }

        return MapToTaskResponse(createdTask);
    }

    public async Task<TaskResponse> UpdateAsync(int taskId, UpdateTaskRequest request, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title is required");
        }
        
        var task = await _taskRepository.GetByIdAndOwnerIdAsync(taskId, ownerId);

        if (task is null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        if (request.AssignedUserId.HasValue && request.AssignedUserId.Value != ownerId)
        {
            throw new  InvalidOperationException("You can only assign tasks to yourself in this version.");
        }
        
        task.Title = request.Title.Trim();
        task.Description = request.Description?.Trim();
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.AssignedUserId = request.AssignedUserId;
        
        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync();
        
        var updatedTask = await _taskRepository.GetByIdAndOwnerIdAsync(task.Id, ownerId);

        if (updatedTask is null)
        {
            throw new InvalidOperationException("Task was created but could not be loaded");
        }
        return MapToTaskResponse(updatedTask);
    }

    public async Task<TaskResponse> UpdateStatusAsync(int taskId, UpdateTaskStatusRequest request, Guid ownerId)
    {
        var task = await _taskRepository.GetByIdAndOwnerIdAsync(taskId, ownerId);

        if (task is null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        task.Status = request.Status;
        _taskRepository.Update(task);
        
        return MapToTaskResponse(task);
        
    }

    public async Task DeleteAsync(int taskId, Guid ownerId)
    {
        var task = await _taskRepository.GetByIdAndOwnerIdAsync(taskId, ownerId);

        if (task is null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        
        _taskRepository.Delete(task);
        await _taskRepository.SaveChangesAsync();
    }

    private static TaskResponse MapToTaskResponse(ProjectTask task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            ProjectName = task.Project.Name,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            AssignedUserId = task.AssignedUserId,
            AssignedUserEmail = task.AssignedUser?.Email
        };
    }
}