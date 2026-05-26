using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Task;
using TaskManagementApi.Services.Tasks;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] TaskFilter filter)
    {
        List<TaskResponse> tasks = _taskService.GetAll(filter);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        TaskResponse task = _taskService.GetById(id);
        return Ok(task);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateTaskRequest request)
    {
        int createdBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _taskService.Add(request, createdBy);

        return Created();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateTaskRequest request)
    {
        _taskService.Update(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _taskService.Delete(id);
        return NoContent();
    }
}
