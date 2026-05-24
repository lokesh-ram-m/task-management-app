using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Authentication;

public class LoginRequest
{
    [Required(ErrorMessage = "Username/Email is required")]
    public required string Login { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public required string Password { get; set; }
}