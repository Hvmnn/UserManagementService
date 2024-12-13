using Microsoft.EntityFrameworkCore;
using UserManagementService.Src.Models;

namespace UserManagementService.Src.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Career> Careers { get; set; } = null!;
        public DbSet<UserProgress> UsersProgresses { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
    }
}