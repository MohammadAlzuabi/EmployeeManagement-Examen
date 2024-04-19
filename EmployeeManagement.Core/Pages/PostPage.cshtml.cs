using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages
{
    public class PsotPageModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        [BindProperty]
        public Post Post { get; set; }
        public int UserId { get; set; }

        public List<CustomPostModel> Posts { get; set; }

        public class CustomPostModel
        {
            public string Id { get; set; }

            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public PsotPageModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        public async Task<IActionResult> OnGetAsync(int postId,int userId)
        {
            //Post = await _httpService.HttpGetRequestById(postId, userId);
            return Page();
        }
    }
}
