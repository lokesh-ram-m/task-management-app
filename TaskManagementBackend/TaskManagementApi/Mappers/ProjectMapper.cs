using TaskManagementApi.DTOs.Project;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers;

// ── ProjectMapper ─────────────────────────────────────────────────────────────
// Why no ToResponse() here?
// ProjectResponse needs CreatedByUsername — not just CreatedBy ID.
// This comes from a JOIN query in the repository (Projects JOIN Users).
// Dapper maps the JOIN result directly to ProjectResponse — no mapper needed.
//
// This mapper only handles CreateProjectRequest → Project entity (for INSERT)
// ─────────────────────────────────────────────────────────────────────────────

public static class ProjectMapper
{
    // CreateProjectRequest DTO → Project entity
    // createdBy comes from JWT token (not from client request)
    // CreatedAt is set by server — not client
    public static Project ToEntity(this CreateProjectRequest dto, int createdBy)
    {
        return new Project
        {
            Name        = dto.Name,
            Description = dto.Description,
            CreatedBy   = createdBy
        };
    }
}