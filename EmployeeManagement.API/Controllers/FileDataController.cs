using EmployeeManagement.API.Repository;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDataController : ControllerBase
    {
        private readonly FileDataRepository _fileDataRepository;

        public FileDataController(FileDataRepository fileDataRepository)
        {
            _fileDataRepository = fileDataRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var file = await _fileDataRepository.GetAllAsync();
            if (file is null)
                return NotFound();
            return Ok(file);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FileData>> GetOneAsync([FromRoute] int id)
        {
            var file = await _fileDataRepository.GetOneAsync(id);
            if (file is null)
                return NotFound();
            return Ok(file);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] FileData fileData)
        {
            var create = await _fileDataRepository.CreateAsync(fileData);
            if (!create)
                return NotFound();
            return Ok(create);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] int id, [FromBody] FileData fileData)
        {
            var update = await _fileDataRepository.UpdateAsync(id, fileData);
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _fileDataRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok(deleted);
        }


    }
}
