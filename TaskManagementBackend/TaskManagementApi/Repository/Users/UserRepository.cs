using Dapper;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repository.Users;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config) { }

    public List<User> GetAll()
    {
        using var connection = CreateConnection();
        return connection.Query<User>("SELECT * FROM Users").ToList();
    }

    public User? GetById(int id)
    {
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<User>(
            "SELECT * FROM Users WHERE Id = @Id",
            new { Id = id }
        );
    }

    public User? GetByUsernameOrEmail(string userName, string email)
    {
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<User>(
            "SELECT * FROM Users WHERE Username = @Username OR Email = @Email",
            new { 
                Username = userName,
                Email = email 
            }
        );
    }

    public User? GetByLoginField(string login)
    {
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<User>(
            "SELECT * FROM Users WHERE Username = @Login OR Email = @Login",
            new { Login = login }
        );
    }

    public void Add(User user)
    {
        using var connection = CreateConnection();
        connection.Execute(
            @"INSERT INTO Users (Username, Email, PasswordHash, Role, IsActive, CreatedAt)
              VALUES (@Username, @Email, @PasswordHash, @Role, @IsActive, @CreatedAt)",
            user
        );
    }

    public void Update(User user)
    {
        using var connection = CreateConnection();
        connection.Execute(
            @"UPDATE Users
              SET Username = @Username, Email = @Email, Role = @Role
              WHERE Id = @Id",
            user
        );
    }

    public void Deactivate(int id)
    {
        using var connection = CreateConnection();
        connection.Execute(
            @"UPDATE Users
              SET IsActive = 0
              WHERE Id = @Id",
            new {Id = id}
        );
    }
}