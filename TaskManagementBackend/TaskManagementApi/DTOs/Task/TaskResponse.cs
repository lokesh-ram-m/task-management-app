using TaskStatus = TaskManagementApi.Enums.TaskStatus;

namespace TaskManagementApi.DTOs.Task;

public class TaskResponse
{
    public int        Id                 { get; set; }
    public string     Title              { get; set; } = string.Empty;
    public string     Description        { get; set; } = string.Empty;
    public TaskStatus Status             { get; set; } = TaskStatus.New;

    // AssignedTo — ID for updates, Username for display
    public int        AssignedToId       { get; set; }
    public string     AssignedToUsername { get; set; } = string.Empty;

    // Reporter — ID for updates, Username for display
    public int        ReporterId         { get; set; }
    public string     ReporterUsername   { get; set; } = string.Empty;

    // Project — ID for updates, Name for display
    public int        ProjectId          { get; set; }
    public string     ProjectName        { get; set; } = string.Empty;

    public int        CreatedBy          { get; set; }
    public DateTime   CreatedAt          { get; set; }
    public DateTime   UpdatedAt          { get; set; }
}