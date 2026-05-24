using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Task;

// Client sends this when creating a task
// Status, CreatedBy, CreatedAt, UpdatedAt are set by server — not client
public class CreateTaskRequest
{
    [Required(ErrorMessage = "Title is required")]
    public string Title        { get; set; } = string.Empty;

    public string Description  { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "AssignedToId must be a valid user")]
    public int    AssignedToId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ReporterId must be a valid user")]
    public int    ReporterId   { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProjectId must be a valid project")]
    public int    ProjectId    { get; set; }
}