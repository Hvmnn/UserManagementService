using UserManagementService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Src.Data
{
    public static class Seeder
    {
        public static async Task SeedAsync(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var now = DateTime.UtcNow;

            var adminRole = new Role
            {
                Name = "Admin",
                Description = "Administrator role",
                CreatedAt = now,
                UpdatedAt = now
            };
            var userRole = new Role
            {
                Name = "User",
                Description = "Regular user role",
                CreatedAt = now,
                UpdatedAt = now
            };
            context.Roles.AddRange(adminRole, userRole);
            await context.SaveChangesAsync();

            var career1 = new Career
            {
                Name = "Computer Science",
                CreatedAt = now,
                UpdatedAt = now
            };
            var career2 = new Career
            {
                Name = "Electrical Engineering",
                CreatedAt = now,
                UpdatedAt = now
            };
            context.Careers.AddRange(career1, career2);
            await context.SaveChangesAsync();

            var subject1 = new Subject
            {
                Code = "CS101",
                Name = "Introduction to Programming",
                Department = "Computer Science",
                Credits = 3,
                Semester = 1,
                CreatedAt = now,
                UpdatedAt = now
            };
            var subject2 = new Subject
            {
                Code = "CS102",
                Name = "Data Structures",
                Department = "Computer Science",
                Credits = 4,
                Semester = 2,
                CreatedAt = now,
                UpdatedAt = now
            };
            var subject3 = new Subject
            {
                Code = "EE101",
                Name = "Basic Circuits",
                Department = "Electrical Engineering",
                Credits = 3,
                Semester = 1,
                CreatedAt = now,
                UpdatedAt = now
            };
            context.Subjects.AddRange(subject1, subject2, subject3);
            await context.SaveChangesAsync();

            var user1 = new User
            {
                Name = "John",
                FirstLastName = "Doe",
                SecondLastName = "Smith",
                RUT = "12345678-9",
                Email = "john.doe@example.com",
                HashedPassword = "hashedpassword123",
                IsEnabled = true,
                CareerId = career1.Id,
                RoleId = userRole.Id,
                CreatedAt = now,
                UpdatedAt = now
            };
            var user2 = new User
            {
                Name = "Jane",
                FirstLastName = "Doe",
                SecondLastName = "Johnson",
                RUT = "98765432-1",
                Email = "jane.doe@example.com",
                HashedPassword = "hashedpassword456",
                IsEnabled = true,
                CareerId = career2.Id,
                RoleId = adminRole.Id,
                CreatedAt = now,
                UpdatedAt = now
            };
            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            var progress1 = new UserProgress
            {
                UserId = user1.Id,
                SubjectId = subject1.Id,
                CreatedAt = now,
                UpdatedAt = now
            };
            var progress2 = new UserProgress
            {
                UserId = user1.Id,
                SubjectId = subject2.Id,
                CreatedAt = now,
                UpdatedAt = now
            };
            var progress3 = new UserProgress
            {
                UserId = user2.Id,
                SubjectId = subject3.Id,
                CreatedAt = now,
                UpdatedAt = now
            };
            context.UsersProgresses.AddRange(progress1, progress2, progress3);
            await context.SaveChangesAsync();
        }
    }
}
