using UserManagementService.Src.Models;

namespace UserManagementService.Src.Repositories.Interfaces
{
    public interface ISubjectsRepository
    {
        Task<List<Subject>> GetAllSubjectsAsync();
    }
}