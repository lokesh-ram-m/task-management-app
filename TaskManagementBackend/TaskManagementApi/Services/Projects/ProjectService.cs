using TaskManagementApi.DTOs.Project;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;
using TaskManagementApi.Repository.Projects;

namespace TaskManagementApi.Services.Projects;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public List<ProjectResponse> GetAll()
    {
        return _projectRepository.GetAll();
    }

    public ProjectResponse GetById(int id)
    {
        ProjectResponse? project = _projectRepository.GetById(id);
        if (project == null)
            throw new Exception($"Project with id {id} not found.");

        return project;
    }

    public List<ProjectResponse> GetByUserId(int userId)
    {
        return _projectRepository.GetByUserId(userId);
    }

    public void Add(CreateProjectRequest request, int createdBy)
    {
        // createdBy comes from JWT claims in the controller — not from the client body
        Project project = request.ToEntity(createdBy);
        _projectRepository.Add(project);
    }

    public void Update(int id, CreateProjectRequest request)
    {
        ProjectResponse? existing = _projectRepository.GetById(id);
        if (existing == null)
            throw new Exception($"Project with id {id} not found.");

        Project project = request.ToEntity(existing.CreatedBy);
        project.Id        = id;
        project.CreatedAt = existing.CreatedAt;
        project.UpdatedAt = DateTime.UtcNow;

        _projectRepository.Update(project);
    }

    public void Delete(int id)
    {
        ProjectResponse? project = _projectRepository.GetById(id);
        if (project == null)
            throw new Exception($"Project with id {id} not found.");

        _projectRepository.Delete(id);
    }
}
