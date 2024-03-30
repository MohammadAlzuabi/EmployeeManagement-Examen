using EmployeeManagement.API.Repository;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController: ControllerBase
    {
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentController(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var department = await _departmentRepository.GetAsync();
            if (department is null)
                return NotFound();
            return Ok(department);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Department>> GetByNameAsync([FromRoute] string name)
        {
            var department = await _departmentRepository.GetByNameAsync(name);
            if (department is null)
                return NotFound();
            return Ok(department);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Department department)
        {
            var create = await _departmentRepository.CreateAsync(department);
            if (!create)
                return NotFound();
            return Ok(create);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] string name, [FromBody] Department department)
        {
            var update = await _departmentRepository.UpdateAsync(name, department);
            var newDepartment = await GetByNameAsync(name);
            return Ok(newDepartment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _departmentRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok(deleted);
        }
    }
}
