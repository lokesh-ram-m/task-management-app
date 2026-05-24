using TaskManagementApi.DTOs.Task;
using TaskManagementApi.Mappers;
using TaskManagementApi.Repository.Tasks;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public List<TaskResponse> GetAll(TaskFilter filter)
    {
        return _taskRepository.GetAll(filter);
    }

    public TaskResponse GetById(int id)
    {
        TaskResponse? task = _taskRepository.GetById(id);
        if (task == null)
            throw new Exception($"Task with id {id} not found.");

        return task;
    }

    public void Add(CreateTaskRequest request, int createdBy)
    {
        // createdBy comes from JWT claims in the controller — not from the client body
        Task task = request.ToEntity(createdBy);
        _taskRepository.Add(task);
    }

    public void Update(int id, CreateTaskRequest request)
    {
        TaskResponse? existing = _taskRepository.GetById(id);
        if (existing == null)
            throw new Exception($"Task with id {id} not found.");

        // Map updated fields onto the existing task
        Task task = request.ToEntity(existing.CreatedBy);
        task.Id        = id;
        task.CreatedAt = existing.CreatedAt;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);
    }

    public void Delete(int id)
    {
        TaskResponse? task = _taskRepository.GetById(id);
        if (task == null)
            throw new Exception($"Task with id {id} not found.");

        _taskRepository.Delete(id);
    }
}
