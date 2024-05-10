using EmployeeManagement.API.DAL;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class UserRepository
    {
        private readonly MyDbContext _context;
        public UserRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAsync() => await _context.Users.Include(x => x.Role).Include(x => x.Department).ToListAsync();

        public async Task<User> GetOneAsync(int id) => await _context.Users.Where(x => x.Id == id).Include(u => u.Role).Include(x => x.Department).FirstOrDefaultAsync();
        public async Task<bool> CreateAsync(User user)
        {
            if (user != null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



        public async Task<User> GetUserByRoleAdmin(string roleName) => await _context.Users
            .Include(x => x.Role).Where(x => x.Role.Name == roleName).FirstOrDefaultAsync();
        public async Task<User> GetUserByRole(string roleName, int userId) => await _context.Users.Include(x => x.Role)
            .Where(x => x.Role.Name == roleName && x.Id == userId).SingleOrDefaultAsync();
        public async Task<bool> UpdatetAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Department = user.Department;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;
                existingUser.ProfileImg = user.ProfileImg;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
