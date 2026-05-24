using TaskManagementApi.Models;

namespace TaskManagementApi.Repository.Users;

public interface IUserRepository
{
    List<User>  GetAll();
    User?       GetById(int id);
    User?       GetByUsernameOrEmail(string userName, string email);
    User?       GetByLoginField(string login);
    void        Add(User user);
    void        Update(User user);
    void        Deactivate(int id);
}