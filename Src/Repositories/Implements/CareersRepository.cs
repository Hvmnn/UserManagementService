using Microsoft.EntityFrameworkCore;
using UserManagementService.Src.Data;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Interfaces;

namespace UserManagementService.Src.Repositories.Implements
{
    public class CareersRepository : ICareersRepository
    {
        private readonly DataContext _context;

        public CareersRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Career?> GetByIdAsync(int careerId)
        {
            return await _context.Careers.FirstOrDefaultAsync(c => c.Id == careerId);
        }
    }
}