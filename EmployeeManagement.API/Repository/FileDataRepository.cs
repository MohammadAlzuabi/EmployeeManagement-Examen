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
        public async Task<IEnumerable<FileData>> GetAllAsync() => await _context.Files.ToListAsync();


        public async Task<FileData> GetOneAsync(int id) => await _context.Files.Where(x => x.Id == id).Include(u => u.User).FirstOrDefaultAsync();

        public async Task<bool> CreateAsync(FileData fileData)
        {
            if (fileData != null)
            {
                _context.Files.Add(fileData);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(int id, FileData fileData)
        {
            var existingFile = await _context.Files.FindAsync(id);
            if (existingFile != null)
            {
                existingFile.File = fileData.File;
                _context.Files.Update(existingFile);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file != null)
            {
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



    }
}
