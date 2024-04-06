using EmployeeManagement.API.Repository;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly MessageRepository _messageRepository;
        public MessageController(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var message = await _messageRepository.GetAsync();
            if (message is null)
                return NotFound();
            return Ok(message);
        }
        [HttpGet("UserId/{userId}")]
        public async Task<ActionResult<List<Message>>> GetAllById(int userId)
        {
           var message = await _messageRepository.GetAllById(userId);
            if (message is null)
                return NotFound();
            return Ok(message);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetOneAsync([FromRoute] int id)
        {
            var message = await _messageRepository.GetOneAsync(id);
            if (message is null)
                return NotFound();
            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Message message)
        {
            var create = await _messageRepository.CreateAsync(message);
            if (!create)
                return NotFound();
            return Ok(create);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] int id, [FromBody] Message message)
        {
            var update = await _messageRepository.UpdatetAsync(id, message);
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _messageRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok(deleted);
        }
    }
}
