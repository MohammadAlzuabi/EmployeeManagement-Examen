using EmployeeManagement.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDataController : ControllerBase
    {
        private readonly FileDataController _fileDataController;

        public FileDataController(FileDataController fileDataController)
        {
            _fileDataController = fileDataController;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var file = await _fileDataController.GetAllAsync();
            if (file is null)
                return NotFound();
            return Ok(file);
        }
    }
}
