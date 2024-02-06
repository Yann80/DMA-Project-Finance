using Core.Entities;
using Core.Repositories;
using DMA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DMA.Infrastructure.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskItemRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task CreateTaskItem(TaskItem task, int projectId, decimal amountCharged)
        {
            var project = _context.Projects.Where(p => p.ProjectId == projectId).SingleOrDefault();
            
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            _context.ProjectTasks.Add(new ProjectTask { ProjectId = projectId, TaskItemId = task.TaskItemId, Billing = amountCharged });
            await _context.SaveChangesAsync();
        }

        public Task DeleteTaskItem(int taskId)
        {
            var objTaskItem = _context.TaskItems.Where(t => t.TaskItemId == taskId).SingleOrDefault();

            if(objTaskItem != null)
            {
                _context.TaskItems.Remove(objTaskItem);
                _context.SaveChanges();
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks(Project project)
        {
            return await _context.TaskItems.Where(ti => ti.ProjectTasks.Any(pt => pt.ProjectId == project.ProjectId)).ToListAsync();
        }

        public async Task<TaskItem> GetTaskItemDetails(int taskId)
        {
            return _context.TaskItems.Include(p => p.ProjectTasks).Where(t => t.TaskItemId == taskId).SingleOrDefault();
        }

        public async Task UpdateTaskItem(TaskItem task)
        {
            TaskItem? objTask = null;

            if (!string.IsNullOrEmpty(task.Gid))
            {
                objTask = _context.TaskItems.Where(t => t.Gid == task.Gid).SingleOrDefault();
            }
            else
            {
                objTask = _context.TaskItems.Where(t => t.TaskItemId == task.TaskItemId).SingleOrDefault();
            }
             
            objTask.Completed = task.Completed;
            objTask.CompletedAt = task.CompletedAt;

            _context.TaskItems.Update(objTask);
            await _context.SaveChangesAsync();
        }
    }
}
