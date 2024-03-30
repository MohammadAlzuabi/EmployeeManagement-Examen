using EmployeeManagement.API.DAL;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class DepartmentRepository
    {
        private readonly MyDbContext _context;
        public DepartmentRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAsync() => await _context.Departments.ToListAsync();

        public async Task<Department> GetByNameAsync(string name) => await _context.Departments.FirstOrDefaultAsync(x => x.Name == name);
        public async Task<bool> CreateAsync(Department department)
        {
            if (department != null)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(string name, Department department)
        {
            var existingDepartment = await _context.Departments.FindAsync(name);
            if (existingDepartment != null)
            {
                existingDepartment.Name = department.Name;
                _context.Departments.Update(existingDepartment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
