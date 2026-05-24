using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Project;

// CreatedBy, CreatedAt, UpdatedAt are set by server — not client
public class CreateProjectRequest
{
    [Required(ErrorMessage = "Project name is required")]
    public string Name        { get; set; } = string.Empty;

    public string Description  { get; set; } = string.Empty;
}