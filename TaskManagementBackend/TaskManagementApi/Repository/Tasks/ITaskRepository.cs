using TaskManagementApi.DTOs.Task;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Repository.Tasks;

public interface ITaskRepository
{
    List<TaskResponse>  GetAll(TaskFilter filter);   // returns DTO — JOIN query maps directly
    TaskResponse?       GetById(int id);
    void                Add(Task task);
    void                Update(Task task);
    void                Delete(int id);
}
