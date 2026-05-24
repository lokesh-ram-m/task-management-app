using TaskManagementApi.DTOs.Authentication;
using TaskManagementApi.DTOs.User;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers;

// ── UserMapper ────────────────────────────────────────────────────────────────
// Handles all User DTO ↔ Entity conversions
//
// Why does UserMapper have ToResponse but TaskMapper and ProjectMapper don't?
// TaskResponse and ProjectResponse need data from multiple tables
// (AssignedToUsername, ReporterUsername, ProjectName, CreatedByUsername).
// These come from JOIN queries in the repository — Dapper maps directly to
// the DTO. No separate mapper needed for those responses.
//
// UserResponse only needs User table data — simple mapper works here.
// ─────────────────────────────────────────────────────────────────────────────

public static class UserMapper
{
    // RegisterRequest DTO → User entity
    // Password is hashed here — never store plain text password
    public static User ToEntity(this RegisterRequest dto)
    {
        return new User
        {
            Username     = dto.Username,
            Email        = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
    }

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