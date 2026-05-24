using TaskManagementApi.DTOs.Authentication;

namespace TaskManagementApi.Services.Auth;

public interface IAuthService
{
    LoginResponse  Login(LoginRequest request);
    void           Register(RegisterRequest request);
}
