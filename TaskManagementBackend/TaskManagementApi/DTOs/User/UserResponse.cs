using TaskManagementApi.Enums;

namespace TaskManagementApi.DTOs.User;

public class UserResponse
{
    public int      Id       { get; set; }
    public string   Username { get; set; } = string.Empty;
    public string   Email    { get; set; } = string.Empty;
    public UserRole Role     { get; set; }
    public bool     IsActive { get; set; }
}