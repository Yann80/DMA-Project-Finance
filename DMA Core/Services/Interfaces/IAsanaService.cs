using Core.DTO;
using Core.Entities;

namespace DMA_ProjectManagement.Core.Services.Interfaces
{
    public interface IAsanaService
    {
        Task<List<Project>> GetAllProjectsFromApi();
        Task<List<Project>> GetAllProjectFromDb();
        Task<List<TaskItem>> GetProjectTasksFromApi(string gid);
        Task<List<TaskItem>> GetAllProjectTasksFromDb(int projectId);
        Task ImportProjectTasks(int projectId);
    }
}
