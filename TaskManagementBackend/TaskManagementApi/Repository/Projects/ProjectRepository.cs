using Dapper;
using TaskManagementApi.DTOs.Project;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repository.Projects;

public class ProjectRepository : BaseRepository, IProjectRepository
{
    public ProjectRepository(IConfiguration config) : base(config) { }

    public List<ProjectResponse> GetAll()
    {
        using var connection = CreateConnection();
        return connection.Query<ProjectResponse>(
            @"SELECT p.Id, p.Name, p.Description,
                     p.CreatedBy, u.Username AS CreatedByUsername,
                     p.CreatedAt, p.UpdatedAt
              FROM Projects p
              INNER JOIN Users u ON p.CreatedBy = u.Id"
        ).ToList();
    }

    public ProjectResponse? GetById(int id)
    {
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<ProjectResponse>(
            @"SELECT p.Id, p.Name, p.Description,
                     p.CreatedBy, u.Username AS CreatedByUsername,
                     p.CreatedAt, p.UpdatedAt
              FROM Projects p
              INNER JOIN Users u ON p.CreatedBy = u.Id
              WHERE p.Id = @Id",
            new { Id = id }
        );
    }

    // Returns all projects where the user has at least one task assigned
    public List<ProjectResponse> GetByUserId(int userId)
    {
        using var connection = CreateConnection();
        return connection.Query<ProjectResponse>(
            @"SELECT DISTINCT p.Id, p.Name, p.Description,
                     p.CreatedBy, u.Username AS CreatedByUsername,
                     p.CreatedAt, p.UpdatedAt
              FROM Projects p
              INNER JOIN Users   u ON p.CreatedBy    = u.Id
              INNER JOIN Tasks   t ON t.ProjectId    = p.Id
              WHERE t.AssignedToId = @UserId",
            new { UserId = userId }
        ).ToList();
    }

    public void Add(Project project)
    {
        using var connection = CreateConnection();
        connection.Execute(
            @"INSERT INTO Projects (Name, Description, CreatedBy, CreatedAt, UpdatedAt)
              VALUES (@Name, @Description, @CreatedBy, @CreatedAt, @UpdatedAt)",
            project
        );
    }

    public void Update(Project project)
    {
        using var connection = CreateConnection();
        connection.Execute(
            @"UPDATE Projects
              SET Name        = @Name,
                  Description = @Description,
                  UpdatedAt   = @UpdatedAt
              WHERE Id = @Id",
            project
        );
    }

    public void Delete(int id)
    {
        using var connection = CreateConnection();
        connection.Execute(
            "DELETE FROM Projects WHERE Id = @Id",
            new { Id = id }
        );
    }
}
