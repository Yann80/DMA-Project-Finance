using Core.DTO;
using Core.Entities;

namespace Core.Repositories
{
    public interface IImportRepository
    {
        Task ImportProject(List<Project> onlineProjects,string userName);
        Task ImportProjectTask(int projectId,List<TaskItem> taskItem);
    }
}
