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
            var myFile = await _httpService.HttpGetRequest<FileData>($"FileData/{id}");
            if (myFile == null || myFile.File == null)
            {
                return NotFound();
            }

            byte[] byteArr = myFile.File;
            string mimeType = "application/pdf";
            return new FileContentResult(byteArr, mimeType);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            var myFile = await _httpService.HttpGetRequest<FileData>($"FileData/{id}");
            if (myFile == null || myFile.File == null)
            {
                return NotFound();
            }

            await _httpService.HttpDeleteRequest<User>($"FileData/{id}");

            return RedirectToPage();
        }
    }
}
