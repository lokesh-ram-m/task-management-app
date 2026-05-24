using TaskStatus = TaskManagementApi.Enums.TaskStatus;

namespace TaskManagementApi.Models;

// Model is never created directly from user input — always via Mapper from DTO
// So 'required' is not needed here. DB constraints handle null enforcement.
public class Task
{
    public int        Id           { get; set; }
    public string     Title        { get; set; } = string.Empty;
    public string     Description  { get; set; } = string.Empty;
    public TaskStatus Status       { get; set; } = TaskStatus.New;
    public int        AssignedToId { get; set; }
    public int        ReporterId   { get; set; }
    public int        ProjectId    { get; set; }
    public int        CreatedBy    { get; set; }
    public DateTime   CreatedAt    { get; set; } = DateTime.UtcNow;
    public DateTime   UpdatedAt    { get; set; } = DateTime.UtcNow;
}