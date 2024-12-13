using Microsoft.EntityFrameworkCore;
using UserManagementService.Src.Data;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Interfaces;

namespace UserManagementService.Src.Repositories.Implements
{
    public class SubjectsRepository : ISubjectsRepository
    {
        private readonly DataContext _context;

        public SubjectsRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects.ToListAsync();
        }
    }
}