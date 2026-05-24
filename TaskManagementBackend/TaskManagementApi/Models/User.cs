using TaskManagementApi.Enums;

namespace TaskManagementApi.Models;

// Model is never created directly from user input — always via Mapper from DTO
// So 'required' is not needed here. DB constraints handle null enforcement.
public class User
{
    public int      Id           { get; set; }
    public string   Username     { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public string   PasswordHash { get; set; } = string.Empty;
    public UserRole Role         { get; set; } = UserRole.User;
    public bool     IsActive     { get; set; } = true;
    public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;
}