using Core.Entities;

namespace Core.Services.Interfaces
{
    public interface IImportService
    {
        Task<bool> ImportProjectTask(string userName);
    }
}
