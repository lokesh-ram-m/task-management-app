namespace TaskManagementApi.Models;

// Model is never created directly from user input — always via Mapper from DTO
// So 'required' is not needed here. DB constraints handle null enforcement.
public class Project
{
    public int      Id          { get; set; }
    public string   Name        { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public int      CreatedBy   { get; set; }
    public DateTime CreatedAt   { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt   { get; set; } = DateTime.UtcNow;
}