using EmployeeManagement.API.DAL;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class FileDataRepository
    {
        private MyDbContext _context;
        public FileDataRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FileData>> GetAsync() => await _context.Files.ToListAsync();
    }
}
