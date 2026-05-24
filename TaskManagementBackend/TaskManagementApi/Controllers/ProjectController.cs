using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Project;
using TaskManagementApi.Services.Projects;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<ProjectResponse> projects = _projectService.GetAll();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        ProjectResponse project = _projectService.GetById(id);
        return Ok(project);
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetByUserId(int userId)
    {
        List<ProjectResponse> projects = _projectService.GetByUserId(userId);
        return Ok(projects);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateProjectRequest request)
    {
        int createdBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _projectService.Add(request, createdBy);
        return Created();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] CreateProjectRequest request)
    {
        _projectService.Update(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _projectService.Delete(id);
        return NoContent();
    }
}
