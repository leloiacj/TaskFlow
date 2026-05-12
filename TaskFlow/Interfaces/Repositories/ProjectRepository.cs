using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Interfaces.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllByOwnerIdAsync(Guid ownerId)
    {
        return await _context.Projects
            .Where(project => project.OwnerId == ownerId)
            .Include(project => project.Tasks)
            .OrderByDescending(project => project.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAndOwnerIdAsync(int projectId, Guid ownerId)
    {
        return await  _context.Projects
            .Where(project => project.Id == projectId)
            .Include(project => project.Tasks)
            .FirstOrDefaultAsync(project => project.Id == projectId && project.OwnerId == ownerId);
    }

    public async Task<Project?> GetByIdAsync(int projectId)
    {
        return await _context.Projects
            .Include(project => project.Tasks)
            .FirstOrDefaultAsync(project => project.Id == projectId);
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
    }

    public async Task<bool> ExistsForOwnerAsync(int projectId, Guid ownerId)
    {
        return await _context.Projects
            .AnyAsync(project => project.Id == projectId && project.OwnerId == ownerId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}