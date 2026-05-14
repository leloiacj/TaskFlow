using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.DTOs.Tasks;
using TaskFlow.Interfaces.Services;

namespace TaskFlow.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
     private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetAll()
    {
        var ownerId = GetCurrentUserId();

        var tasks = await _taskService.GetAllAsync(ownerId);

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetById(int id)
    {
        var ownerId = GetCurrentUserId();

        var task = await _taskService.GetByIdAsync(id, ownerId);

        return Ok(task);
    }

    [HttpGet("project/{projectId:int}")]
    public async Task<ActionResult<List<TaskResponse>>> GetByProjectId(int projectId)
    {
        var ownerId = GetCurrentUserId();

        var tasks = await _taskService.GetByProjectIdAsync(projectId, ownerId);

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request)
    {
        var ownerId = GetCurrentUserId();

        var task = await _taskService.CreateAsync(request, ownerId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponse>> Update(int id, UpdateTaskRequest request)
    {
        var ownerId = GetCurrentUserId();

        var task = await _taskService.UpdateAsync(id, request, ownerId);

        return Ok(task);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<TaskResponse>> UpdateStatus(int id, UpdateTaskStatusRequest request)
    {
        var ownerId = GetCurrentUserId();

        var task = await _taskService.UpdateStatusAsync(id, request, ownerId);

        return Ok(task);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ownerId = GetCurrentUserId();

        await _taskService.DeleteAsync(id, ownerId);

        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User id claim is missing.");
        }

        return Guid.Parse(userId);
    }
}