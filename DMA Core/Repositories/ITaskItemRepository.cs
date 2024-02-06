using Core.Entities;

namespace Core.Repositories
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasks(Project project);
        Task<TaskItem> GetTaskItemDetails(int taskId);
        Task CreateTaskItem(TaskItem task,int projectId, decimal amountCharged);
        Task DeleteTaskItem(int taskId);
        Task UpdateTaskItem(TaskItem task);
    }
}
