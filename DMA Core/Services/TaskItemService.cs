using Core.Entities;
using Core.Repositories;
using Core.Services.Interfaces;

namespace Core.Services
{
    public class TaskItemService: ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public TaskItemService(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        public async Task DeleteTaskItem(int taskItemId)
        {
            await _taskItemRepository.DeleteTaskItem(taskItemId);
        }

        public async Task UpdateTaskItem(TaskItem taskItem)
        {
            await _taskItemRepository.UpdateTaskItem(taskItem);
        }

        public async Task CreateTaskItem(TaskItem taskItem, int projectId, decimal amountCharged)
        {
            await _taskItemRepository.CreateTaskItem(taskItem, projectId, amountCharged);

        }

        public async Task<TaskItem> GetTaskItem(int taskItemId)
        {
            return await _taskItemRepository.GetTaskItemDetails(taskItemId);
        }
    }
}
