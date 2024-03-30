using EmployeeManagement.API.DAL;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class RoleRepository
    {
        private readonly MyDbContext _context;
        public RoleRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAsync() => await _context.Roles.ToListAsync();

        public async Task<Role> GetOneAsync(int id) => await _context.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Role> GetByNameAsync(string name) => await _context.Roles.FirstOrDefaultAsync(x => x.Name == name);
        public async Task<bool> CreateAsync(Role role)
        {
            if (role != null)
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatetAsync(int id, Role role)
        {
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole != null)
            {
                existingRole.Name = role.Name;
                _context.Roles.Update(existingRole);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
