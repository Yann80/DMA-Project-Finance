using Core.Entities;
using Core.Repositories;
using DMA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DMA.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task CreateProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public Task DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Project>> GetAllSavedProjects()
        {
            return _context.Projects.Include(pt => pt.ProjectTasks).OrderBy(p => p.ProjectName).ToList();
        }

        public async Task<Project> GetProjectById(int id)
        {
            return _context.Projects.Where(p => p.ProjectId == id).SingleOrDefault();
        }

        public async Task<List<TaskItem>> GetProjectTasks(int projectId)
        {
            return _context.TaskItems.Include(t => t.ProjectTasks).Where(ti => ti.ProjectTasks.Any(pt => pt.ProjectId == projectId)).ToList();
        }

        public async Task<Project> UpdateProject(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }
    }
}
