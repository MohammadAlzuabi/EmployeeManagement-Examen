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

        [BindProperty]
        public int Id { get; set; }

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

            if (file != null)
            {
                if (file.Length > 0 && file.Length < 30000 || file.Length > 30000)
                {
                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        FileData.File = target.ToArray();

                    }
                    if (user != null)
                    {
                        FileData.UserId = userId;
                    }
                    if (FileData.UserId != null && FileData.File != null && FileData.Name != null)
                    {
                        await _httpService.HttpPostRequest($"FileData", FileData);

                    }
                }
            }
            return RedirectToPage("/FilePage");
        }
        private async Task<User> GetUserById(int userId)
        {
            return await _httpService.HttpGetRequest<User>($"User/{userId}");
        }
    }
}
