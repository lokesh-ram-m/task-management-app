using TaskManagementApi.DTOs.Task;
using TaskStatus = TaskManagementApi.Enums.TaskStatus;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Mappers;

// ── TaskMapper ────────────────────────────────────────────────────────────────
// Why no ToResponse() here?
// TaskResponse needs usernames and project name — not just IDs.
// These come from JOIN queries in the repository (Tasks JOIN Users JOIN Projects).
// Dapper maps the JOIN result directly to TaskResponse — no mapper needed.
//
// This mapper only handles CreateTaskRequest → Task entity (for INSERT)
// because that's where we need to convert client input to a DB model.
// ─────────────────────────────────────────────────────────────────────────────

public static class TaskMapper
{
    // CreateTaskRequest DTO → Task entity
    // createdBy comes from JWT token (not from client request)
    // Status defaults to New — client cannot set status on creation
    // CreatedAt and UpdatedAt are set by server — not client
    public static Task ToEntity(this CreateTaskRequest dto, int createdBy)
    {
        return new Task
        {
            Title        = dto.Title,
            Description  = dto.Description,
            AssignedToId = dto.AssignedToId,
            ReporterId   = dto.ReporterId,
            ProjectId    = dto.ProjectId,
            CreatedBy    = createdBy,        // extracted from JWT claims in service
            Status       = TaskStatus.New,   // always starts as New
            CreatedAt    = DateTime.UtcNow,
            UpdatedAt    = DateTime.UtcNow
        };
    }
}