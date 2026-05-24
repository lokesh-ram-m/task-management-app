using TaskManagementApi.DTOs.User;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;
using TaskManagementApi.Repository.Users;

namespace TaskManagementApi.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<UserResponse> GetAll()
    {
        return _userRepository.GetAll().ToResponseList();
    }

    public UserResponse GetById(int id)
    {
        // Throw if not found — controller will catch and return 404
        User? user = _userRepository.GetById(id);
        if (user == null)
            throw new Exception($"User with id {id} not found.");

        return user.ToResponse();
    }

    public void Update(int id, UserResponse request)
    {
        User? user = _userRepository.GetById(id);
        if (user == null)
            throw new Exception($"User with id {id} not found.");

        // Only update allowed fields — role and password are NOT updated here
        user.Username = request.Username;
        user.Email    = request.Email;

        _userRepository.Update(user);
    }

    public void Deactivate(int id)
    {
        User? user = _userRepository.GetById(id);
        if (user == null)
            throw new Exception($"User with id {id} not found.");

        _userRepository.Deactivate(id);
    }
}
