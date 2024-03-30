using EmployeeManagement.API.DAL;
using EmployeeManagement.API.Repository;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var user = await _userRepository.GetAsync();
            if (user is null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetOneAsync([FromRoute] int id)
        {
            var user = await _userRepository.GetOneAsync(id);
            if (user is null)
                return NotFound();
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] User user)
        {
            var create = await _userRepository.CreateAsync(user);
            if (!create)
                return NotFound();
            return Ok(create);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] int id, [FromBody] User user)
        {
            var update = await _userRepository.UpdatetAsync(id, user);
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _userRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok(deleted);
        }

    }
}
