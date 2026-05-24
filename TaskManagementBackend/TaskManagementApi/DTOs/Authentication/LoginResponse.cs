using TaskManagementApi.Enums;

namespace TaskManagementApi.DTOs.Authentication;

public class LoginResponse
{
    public string   Token     { get; set; } = string.Empty;
    public string   Username  { get; set; } = string.Empty;
    public UserRole Role      { get; set; }
    public DateTime ExpiresAt { get; set; }
}