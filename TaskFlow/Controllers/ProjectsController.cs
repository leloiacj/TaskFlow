using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.DTOs.Projects;
using TaskFlow.Interfaces.Services;
using TaskFlow.Models;

namespace TaskFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    
    public  ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectResponse>>> GetAll()
    {
        var ownerId = GetCurrentUserId();
        
        var projects = await _projectService.GetAllAsync(ownerId);
        
        return Ok(projects);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponse>> GetById(int id)
    {
        var ownerId = GetCurrentUserId();
        
        var project = await _projectService.GetByIdAsync(id, ownerId);
        
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<Project>> Create(CreateProjectRequest request)
    {
        var ownerId = GetCurrentUserId();
        
        var project = await _projectService.CreateAsync(request, ownerId);
        
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Project>> Update(int id, UpdateProjectRequest request)
    {
        var ownerId = GetCurrentUserId();
        
        var project = await _projectService.UpdateAsync(id, request, ownerId);
        
        return Ok(project);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var ownerId = GetCurrentUserId();
        
        await  _projectService.DeleteAsync(id, ownerId);
        
        return Ok(); 
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User id claim is missing");
        }
        return Guid.Parse(userId);
    }
    
    
    
}