using Microsoft.EntityFrameworkCore;
using UserManagementService.Src.Data;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Interfaces;

namespace UserManagementService.Src.Repositories.Implements
{
    public class CareerRepository : ICareerRepository
    {
        private readonly DataContext _context;

        public CareerRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Career?> GetByIdAsync(int careerId)
        {
            return await _context.Careers.FirstOrDefaultAsync(c => c.Id == careerId);
        }
    }
}