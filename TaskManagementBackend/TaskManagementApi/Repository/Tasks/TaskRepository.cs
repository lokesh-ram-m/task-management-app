using Dapper;
using TaskManagementApi.DTOs.Task;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Repository.Tasks;

public class TaskRepository : BaseRepository, ITaskRepository
{
    public TaskRepository(IConfiguration config) : base(config) { }

    public List<TaskResponse> GetAll(TaskFilter filter)
    {
        using var connection = CreateConnection();

        // Base query — JOINs Users twice (assigned user + reporter) and Projects
        // au = assigned user, ru = reporter user, p = project
        var sql = @"SELECT t.Id, t.Title, t.Description, t.Status,
                           t.AssignedToId, au.Username AS AssignedToUsername,
                           t.ReporterId,   ru.Username AS ReporterUsername,
                           t.ProjectId,     p.Name     AS ProjectName,
                           t.CreatedBy, t.CreatedAt, t.UpdatedAt
                    FROM Tasks t
                    INNER JOIN Users   au ON t.AssignedToId = au.Id
                    INNER JOIN Users   ru ON t.ReporterId   = ru.Id
                    INNER JOIN Projects p ON t.ProjectId    = p.Id
                    WHERE 1=1";

        // DynamicParameters lets us add params conditionally
        var parameters = new DynamicParameters();

        if (filter.AssignedToIds?.Any() == true)
        {
            sql += " AND t.AssignedToId IN @AssignedToIds";
            parameters.Add("AssignedToIds", filter.AssignedToIds);
        }

        if (filter.Statuses?.Any() == true)
        {
            sql += " AND t.Status IN @Statuses";
            parameters.Add("Statuses", filter.Statuses);
        }

        if (filter.ProjectId.HasValue)
        {
            sql += " AND t.ProjectId = @ProjectId";
            parameters.Add("ProjectId", filter.ProjectId.Value);
        }

        return connection.Query<TaskResponse>(sql, parameters).ToList();
    }

    public TaskResponse? GetById(int id)
    {
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<TaskResponse>(
            @"SELECT t.Id, t.Title, t.Description, t.Status,
                     t.AssignedToId, au.Username AS AssignedToUsername,
                     t.ReporterId,   ru.Username AS ReporterUsername,
                     t.ProjectId,     p.Name     AS ProjectName,
                     t.CreatedBy, t.CreatedAt, t.UpdatedAt
              FROM Tasks t
              INNER JOIN Users   au ON t.AssignedToId = au.Id
              INNER JOIN Users   ru ON t.ReporterId   = ru.Id
              INNER JOIN Projects p ON t.ProjectId    = p.Id
              WHERE t.Id = @Id",
            new { Id = id }
        );
    }

    public void Add(Task task)
    {
        using var connection = CreateConnection();
        connection.Execute(
            @"INSERT INTO Tasks (Title, Description, Status, AssignedToId, ReporterId, ProjectId, CreatedBy, CreatedAt, UpdatedAt)
              VALUES (@Title, @Description, @Status, @AssignedToId, @ReporterId, @ProjectId, @CreatedBy, @CreatedAt, @UpdatedAt)",
            task
        );
    }

    public void Update(Task task)
    {
        using var connection = CreateConnection();
        connection.Execute(
            @"UPDATE Tasks
              SET Title        = @Title,
                  Description  = @Description,
                  Status       = @Status,
                  AssignedToId = @AssignedToId,
                  UpdatedAt    = @UpdatedAt
              WHERE Id = @Id",
            task
        );
    }

    public void Delete(int id)
    {
        using var connection = CreateConnection();
        connection.Execute(
            "DELETE FROM Tasks WHERE Id = @Id",
            new { Id = id }
        );
    }
}
