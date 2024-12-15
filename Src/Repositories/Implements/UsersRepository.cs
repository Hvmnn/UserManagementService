using Microsoft.EntityFrameworkCore;
using UserManagementService.Src.Data;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Interfaces;

namespace UserManagementService.Src.Repositories.Implements
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;

        public UsersRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task<bool> AddProgressAsync(List<UserProgress> userProgress)
        {
            await _context.UsersProgresses.AddRangeAsync(userProgress);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProgressAsync(List<UserProgress> userProgress, int userId)
        {
            var subjectsIdsToRemove = userProgress.Select(p => p.SubjectId).ToList();

            var result = await _context.UsersProgresses
                .Where(u => u.UserId == userId && subjectsIdsToRemove.Contains(u.SubjectId))
                .ExecuteDeleteAsync() > 0;
            
            return result;
        }

        public async Task<List<UserProgress>?> GetProgressByUserAsync(int userId)
        {
            var userProgress = await _context.UsersProgresses
                                .Where(u => u.UserId == userId)
                                .Include(u => u.User)
                                .Include(u => u.Subject)
                                .ToListAsync();
            return userProgress;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.Career)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Career)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User?> GetUserByRut(string rut)
        {
            var user = await _context.Users
                .Include(u => u.Career)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.RUT == rut);
            return user;
        }
        public async Task<List<User>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Career)
                .Include(u => u.Role)
                .ToListAsync();
            return users;
        }

        public async Task <User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}