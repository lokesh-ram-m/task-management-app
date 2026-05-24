using TaskManagementApi.DTOs.Project;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repository.Projects;

public interface IProjectRepository
{
    List<ProjectResponse>  GetAll();                  // all projects
    ProjectResponse?       GetById(int id);           // nullable — project might not exist
    List<ProjectResponse>  GetByUserId(int userId);   // projects where user has tasks
    void                   Add(Project project);
    void                   Update(Project project);
    void                   Delete(int id);
}
