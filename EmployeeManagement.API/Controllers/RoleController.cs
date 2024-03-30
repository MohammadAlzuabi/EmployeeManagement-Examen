using EmployeeManagement.API.Repository;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleRepository _roleRepository;
        public RoleController(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var role = await _roleRepository.GetAsync();
            if (role is null)
                return NotFound();
            return Ok(role);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Department>> GetOneAsync([FromRoute] int id)
        //{
        //    var role = await _roleRepository.GetOneAsync(id);
        //    if (role is null)
        //        return NotFound();
        //    return Ok(role);
        //}

        [HttpGet("{name}")]

        public async Task<ActionResult> GetByNameAsync([FromRoute] string name)
        {
            var role = await _roleRepository.GetByNameAsync(name);
            if (role is null)
                return NotFound();
            return Ok(role);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Role role)
        {
            var create = await _roleRepository.CreateAsync(role);
            if (!create)
                return NotFound();
            return Ok(create);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] int id, [FromBody] Role role)
        {
            var update = await _roleRepository.UpdatetAsync(id, role);
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _roleRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok(deleted);
        }
    }
}
