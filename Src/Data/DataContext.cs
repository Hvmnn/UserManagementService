using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Src.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
    }
}