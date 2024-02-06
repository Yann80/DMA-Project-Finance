using Core.Entities;
using Core.Repositories;
using Core.Services.Interfaces;
using DMA_ProjectManagement.Core.Services.Interfaces;

namespace Core.Services
{
    public class ImportService : IImportService
    {
        private readonly IImportRepository _importrepository;
        private readonly IAsanaService _asanaService;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskItemRepository _taskItemRepository;

        public ImportService(IImportRepository importRepository,IAsanaService asanaService, IProjectRepository projectRepository, ITaskItemRepository taskItemRepository)
        {
            _importrepository = importRepository;
            _asanaService = asanaService;
            _projectRepository = projectRepository;
            _taskItemRepository = taskItemRepository;

        }

        public async Task<bool> ImportProjectTask(string userName)
        {
            // We update the projects
            var onlineProjects = await _asanaService.GetAllProjectsFromApi();
            await _importrepository.ImportProject(onlineProjects,userName);

            // We update the tasks
            var localProjects = await _projectRepository.GetAllSavedProjects();
            foreach (var project in localProjects)
            {
                var localProjectTasks = await _asanaService.GetAllProjectTasksFromDb(project.ProjectId);
                var onlineProjectTasks = await _asanaService.GetProjectTasksFromApi(project.Gid);

                // We update the db with the missing tasks that have assignees
                var missingProjectTasks = onlineProjectTasks
                    .Where(task => !localProjectTasks.Exists(local => local.Gid == task.Gid))
                    .ToList();

                if (missingProjectTasks.Count() > 0)
                    await _importrepository.ImportProjectTask(project.ProjectId, missingProjectTasks);

                // Check for tasks that have their Completed value updated
                var onlineItemsUpdated = onlineProjectTasks
                    .Where(onlineItem => {
                        var localItem = localProjectTasks.SingleOrDefault(o => o.Gid == onlineItem.Gid);
                        return localItem != null && localItem.Completed != onlineItem.Completed;
                    })
                    .ToList();


                // We sync the existing tasks
                foreach (var taskItem in onlineItemsUpdated)
                {
                    await _taskItemRepository.UpdateTaskItem(taskItem);
                }
            }

            return true;
        }
    }
}
