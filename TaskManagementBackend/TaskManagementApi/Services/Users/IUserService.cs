using TaskManagementApi.DTOs.User;

namespace TaskManagementApi.Services.Users;

public interface IUserService
{
    List<UserResponse>  GetAll();
    UserResponse        GetById(int id);
    void                Update(int id, UserResponse request);
    void                Deactivate(int id);
}
