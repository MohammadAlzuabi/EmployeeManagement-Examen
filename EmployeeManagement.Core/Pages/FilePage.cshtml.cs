using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static EmployeeManagement.Core.Enums.Enums;

namespace EmployeeManagement.Core.Pages
{
    public class FilePageModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        [BindProperty]
        public FileData FileData { get; set; }

        public List<FileData> Files { get; set; }

        [BindProperty]
        public IFormFile file { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public FilePageModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        public async Task<IActionResult> OnGetAsync(int userId)
        {
            await GetUserById(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int userId)
        {
            var user = await GetUserById(userId);

            if (file != null && ValidateFileSize(file))
            {
                FileData.File = await ConvertFileToByteArrayAsync(file);

                if (user != null)
                {
                    FileData.UserId = userId;
                }

                if (FileData.UserId != null && FileData.File != null && FileData.Name != null)
                {
                    await UploadFileDataAsync(FileData);
                    StatusMessage = "Din fil har laddats upp!";
                }
                else
                {
                    StatusMessage = "Det uppstod ett fel vid uppladdning av filen";
                }
            }
            return RedirectToPage("/FilePage");
        }
        private bool ValidateFileSize(IFormFile file)
        {
            return file.Length > 0 && file.Length <= 30000;
        }

        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private async Task UploadFileDataAsync(FileData fileData)
        {
            await _httpService.HttpPostRequest($"FileData", fileData);
        }
        private async Task<User> GetUserById(int userId)
        {
            return await _httpService.HttpGetRequest<User>($"User/{userId}");
        }
    }
}
