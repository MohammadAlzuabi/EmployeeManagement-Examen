using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages
{
    public class UploadFilesModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        public UploadFilesModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        public List<FileData> Files { get; set; }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            Files = await _modelManagement.UpdateListAsync<FileData>();
            return Page();

        }

        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            var fileData = await GetFileDataAsync(id);
            if (fileData == null || fileData.File == null)
            {
                return NotFound();
            }

            return File(fileData.File, "application/pdf");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            var fileData = await GetFileDataAsync(id);
            if (fileData == null || fileData.File == null)
            {
                return NotFound();
            }

            await DeleteFileDataAsync(id);
            return RedirectToPage();
        }
        private async Task<FileData> GetFileDataAsync(int? id)
        {
            return await _httpService.HttpGetRequest<FileData>($"FileData/{id}");
        }
        private async Task DeleteFileDataAsync(int? id)
        {
            await _httpService.HttpDeleteRequest<User>($"FileData/{id}");
        }
    }
}
