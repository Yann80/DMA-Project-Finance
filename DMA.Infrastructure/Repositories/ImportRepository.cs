using Core.DTO;
using Core.Entities;
using Core.Repositories;
using DMA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DMA.Infrastructure.Repositories
{
    public class ImportRepository : IImportRepository
    {
        private readonly ApplicationDbContext _context;

        public ImportRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task ImportProject(List<Project> onlineProjects, string userName)
        {
            var existingProjects = _context.Projects.ToList();
            if (existingProjects.Count == 0)
            {
                foreach(var project in onlineProjects)
                {
                    project.CreatedBy = userName;
                    project.CreatedOn = DateTime.Now;
                    _context.Projects.Add(project);
                }
            }

            if (existingProjects.Count >= onlineProjects.Count)
            {
                //ar missingProjects = existingProjects.Except(onlineProjects).ToList();

                var missingProjects = onlineProjects.Where(project => !existingProjects.Any(ep => ep.Gid == project.Gid)).ToList();

                if (missingProjects != null && missingProjects.Count > 0)
                {
                    foreach (var project in missingProjects)
                    {
                        _context.Projects.Add(project);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task ImportProjectTask(int projectId, List<TaskItem> newTaskItems)
        {
            // Add new task items
            foreach (var newTaskItem in newTaskItems)
            {
                _context.TaskItems.Add(newTaskItem);

                _context.ProjectTasks.Add(new ProjectTask
                {
                    ProjectId = projectId,
                    Task = newTaskItem
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
