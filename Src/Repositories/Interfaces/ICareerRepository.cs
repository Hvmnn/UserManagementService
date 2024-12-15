using UserManagementService.Src.Models;

namespace UserManagementService.Src.Repositories.Interfaces
{
    public interface ICareerRepository
{
    Task<Career?> GetByIdAsync(int careerId);
}
}