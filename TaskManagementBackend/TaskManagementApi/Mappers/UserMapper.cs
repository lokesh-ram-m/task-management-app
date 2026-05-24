using TaskManagementApi.DTOs.User;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers;

public static class UserMapper
{
    // User entity → UserResponse DTO (hides PasswordHash)
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id       = user.Id,
            Username = user.Username,
            Email    = user.Email,
            Role     = user.Role,
            IsActive = user.IsActive
        };
    }

    // List of Users → List of UserResponse DTOs
    public static List<UserResponse> ToResponseList(this List<User> users)
    {
        return users.Select(u => u.ToResponse()).ToList();
    }
}