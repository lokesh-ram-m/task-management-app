using TaskManagementApi.DTOs.Task;

namespace TaskManagementApi.Services.Tasks;

public interface ITaskService
{
    List<TaskResponse>  GetAll(TaskFilter filter);
    TaskResponse        GetById(int id);
    void                Add(CreateTaskRequest request, int createdBy);
    void                Update(int id, CreateTaskRequest request);
    void                Delete(int id);
}
