using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Src.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Models.User> Users { get; set; } = null!;
        public DbSet<Models.Role> Roles { get; set; } = null!;
        public DbSet<Models.Career> Careers { get; set; } = null!;
        public DbSet<Models.UserProgress> UserProgresses { get; set; } = null!;
        public DbSet<Models.Subject> Subjects { get; set; } = null!;
    }
}