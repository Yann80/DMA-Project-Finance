using Core.Entities;
using Core.Repositories;
using Core.Services.Interfaces;

namespace Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task CreateProject(Project project)
        {
            await _projectRepository.CreateProject(project);
        }

        public Task DeleteProject(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProject(Project project)
        {
            await _projectRepository.UpdateProject(project);
        }

        public async Task<Project> GetProject(int projectId)
        {
            return await _projectRepository.GetProjectById(projectId);
        }
    }
}
