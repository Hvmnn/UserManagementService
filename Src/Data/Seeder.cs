using UserManagementService.Src.Data;
using UserManagementService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Src.Data
{
    public class Seeder
    {
        private readonly DataContext _context;

        public Seeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Users.AnyAsync()) return;

            var career1 = new Career { Name = "Computer Science", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var career2 = new Career { Name = "Electrical Engineering", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _context.Careers.AddRange(career1, career2);
            await _context.SaveChangesAsync();

            var user1 = new User
            {
                Name = "John",
                FirstLastName = "Doe",
                SecondLastName = "Smith",
                Email = "john.doe@example.com",
                RUT = "12345678-9",
                Career = career1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var user2 = new User
            {
                Name = "Jane",
                FirstLastName = "Doe",
                SecondLastName = "Johnson",
                Email = "jane.doe@example.com",
                RUT = "98765432-1",
                Career = career2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            // Add user progress
            var progress1 = new UserProgress { UserId = user1.Id, SubjectId = 101 };
            var progress2 = new UserProgress { UserId = user1.Id, SubjectId = 102 };
            var progress3 = new UserProgress { UserId = user2.Id, SubjectId = 101 };
            _context.UsersProgresses.AddRange(progress1, progress2, progress3);
            await _context.SaveChangesAsync();
        }
    }
}
