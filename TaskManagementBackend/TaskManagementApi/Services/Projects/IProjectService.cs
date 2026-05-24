using TaskManagementApi.DTOs.Project;

namespace TaskManagementApi.Services.Projects;

public interface IProjectService
{
    List<ProjectResponse>  GetAll();
    ProjectResponse        GetById(int id);
    List<ProjectResponse>  GetByUserId(int userId);
    void                   Add(CreateProjectRequest request, int createdBy);
    void                   Update(int id, CreateProjectRequest request);
    void                   Delete(int id);
}
