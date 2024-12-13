using UserManagementService.Src.Models;

namespace UserManagementService.Src.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetUserByIdAsync(int id);
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<User?> GetUserByRut(string rut);
        public Task UpdateUserAsync(User user);
        public Task<bool> AddProgressAsync(List<UserProgress> userProgress);
        public Task<bool> DeleteProgressAsync(List<UserProgress> userProgress, int userId);
        public Task<List<UserProgress>?> GetProgressByUserAsync(int userId);
    }
}