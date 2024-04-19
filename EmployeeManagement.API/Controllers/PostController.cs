using EmployeeManagement.API.Repository;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostRepository _postRepository;

        public PostController(PostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var post = await _postRepository.GetAsync();
            if (post is null)
                return NotFound();
            return Ok(post);
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

        public async Task<ActionResult> GetByIdAsync([FromRoute] int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post is null)
                return NotFound();
            return Ok(post);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Post post)
        {
            var create = await _postRepository.CreateAsync(post);
            if (!create)
                return NotFound();
            return Ok(create);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] int id, [FromBody] Post post)
        {
            var update = await _postRepository.UpdateAsync(id, post);
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _postRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok(deleted);
        }
    }
}
