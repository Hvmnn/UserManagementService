using UserManagementService.Src.Models;

namespace UserManagementService.Src.Repositories.Interfaces
{
    public interface ICareersRepository
{
    Task<Career?> GetByIdAsync(int careerId);
}
}