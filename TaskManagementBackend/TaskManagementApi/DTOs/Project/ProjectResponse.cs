namespace TaskManagementApi.DTOs.Project;

public class ProjectResponse
{
    public int      Id                { get; set; }
    public string   Name              { get; set; } = string.Empty;
    public string   Description       { get; set; } = string.Empty;

    // CreatedBy — ID for lookups, Username for display
    public int      CreatedBy         { get; set; }
    public string   CreatedByUsername { get; set; } = string.Empty;

    public DateTime CreatedAt         { get; set; }
    public DateTime UpdatedAt         { get; set; }
}