using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApi.DTOs.Authentication;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;
using TaskManagementApi.Repository.Users;

namespace TaskManagementApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration  _config;
    private readonly IUserRepository _userRepository;

    public AuthService(IConfiguration config, IUserRepository userRepository)
    {
        _config         = config;
        _userRepository = userRepository;
    }

    public LoginResponse Login(LoginRequest request)
    {
        User? user = _userRepository.GetByLoginField(request.Login);
        if (user == null)
            throw new Exception("Invalid credentials.");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception("Invalid credentials.");

        string token    = GenerateToken(user);
        var    expiresAt = DateTime.UtcNow.AddMinutes(
            double.Parse(_config["Jwt:ExpiresInMinutes"]!)
        );

        return new LoginResponse
        {
            Token     = token,
            Username  = user.Username,
            Role      = user.Role,
            ExpiresAt = expiresAt
        };
    }

    public void Register(RegisterRequest request)
    {
        if (CheckIfUserNameOrEmailExists(request.Username, request.Email))
            throw new Exception("Username or email already taken.");

        User user = request.ToEntity();
        _userRepository.Add(user);
    }

    private string GenerateToken(User user)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,           user.Username),
            new Claim(ClaimTypes.Role,           user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool CheckIfUserNameOrEmailExists(string userName, string email)
    {
        User? user = _userRepository.GetByUsernameOrEmail(userName, email);
        return user != null;
    }
}
