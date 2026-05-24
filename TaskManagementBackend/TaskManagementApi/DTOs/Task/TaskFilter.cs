using TaskStatus = TaskManagementApi.Enums.TaskStatus;

namespace TaskManagementApi.DTOs.Task;

// Filter object for GET /api/tasks query params
// All fields are optional — null means no filter applied for that field
// Using lists allows filtering by multiple users and statuses at once
public class TaskFilter
{
    public List<int>?        AssignedToIds { get; set; }
    public List<TaskStatus>? Statuses      { get; set; }
    public int?              ProjectId     { get; set; }
}