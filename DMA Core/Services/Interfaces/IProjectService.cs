using Core.Entities;

namespace Core.Services.Interfaces
{
    public interface IProjectService
    {
        Task DeleteProject(int projectId);
        Task UpdateProject(Project project);
        Task CreateProject(Project project);
        Task<Project> GetProject(int projectId);
    }
}
