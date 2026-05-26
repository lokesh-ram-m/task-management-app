using System.ComponentModel.DataAnnotations;
using TaskStatus = TaskManagementApi.Enums.TaskStatus;

namespace TaskManagementApi.DTOs.Task;

// Client sends this when updating a task
// Status can be changed by client on update (unlike creation)
public class UpdateTaskRequest
{
    [Required(ErrorMessage = "Title is required")]
    public string     Title        { get; set; } = string.Empty;

    public string     Description  { get; set; } = string.Empty;

    public TaskStatus Status       { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "AssignedToId must be a valid user")]
    public int        AssignedToId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ReporterId must be a valid user")]
    public int        ReporterId   { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProjectId must be a valid project")]
    public int        ProjectId    { get; set; }
}
