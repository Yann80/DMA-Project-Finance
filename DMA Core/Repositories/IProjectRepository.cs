using Core.DTO;
using Core.Entities;

namespace Core.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllSavedProjects();
        Task<List<TaskItem>> GetProjectTasks(int projectId);
        Task<Project> GetProjectById(int id);
        Task<Project> UpdateProject(Project project);
        Task DeleteProject(Project project);
        Task CreateProject(Project project);
    }
}
