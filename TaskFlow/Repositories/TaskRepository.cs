using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Interfaces.Repositories;
using TaskFlow.Models;

namespace TaskFlow.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    
    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ProjectTask>> GetAllByOwenerIdAsync(Guid ownerId)
    {
        return await _context.ProjectTasks
            .Include(task => task.Project)
            .Include(task => task.AssignedUser)
            .Where(task => task.Project.OwnerId == ownerId)
            .OrderByDescending(task => task.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<ProjectTask>> GetAllByProjecIdAndOwnerIdAsync(int projectId, Guid ownerId)
    {
        return await _context.ProjectTasks
            .Include(task => task.Project)
            .Include(task => task.AssignedUser)
            .Where(task => task.ProjectId == projectId && task.Project.OwnerId == ownerId)
            .OrderByDescending(task => task.CreatedAt)
            .ToListAsync();
    }

    public async Task<ProjectTask?> GetByIdAndOwnerIdAsync(int taskId, Guid ownerId)
    {
        return await _context.ProjectTasks
            .Include(task => task.Project)
            .Include(task => task.AssignedUser)
            .FirstOrDefaultAsync(task => task.Id == taskId && task.Project.OwnerId == ownerId);
    }

    public async Task AddAsync(ProjectTask task)
    {
        await _context.ProjectTasks.AddAsync(task);
    }

    public void Update(ProjectTask task)
    {
        _context.ProjectTasks.Update(task);
    }

    public void Delete(ProjectTask task)
    {
        _context.ProjectTasks.Remove(task);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}