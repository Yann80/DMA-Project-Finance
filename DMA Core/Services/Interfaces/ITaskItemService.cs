using Core.Entities;

namespace Core.Services.Interfaces
{
    public interface ITaskItemService
    {
        Task DeleteTaskItem(int taskItemId);
        Task UpdateTaskItem(TaskItem taskItem);
        Task CreateTaskItem(TaskItem taskItem, int projectId, decimal amountCharged);
        Task<TaskItem> GetTaskItem(int taskItemId);
    }
}
