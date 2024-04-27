using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages
{
    public class HomePageModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        [BindProperty]
        public Post Post { get; set; }


        public int CreatedByUser { get; set; }

        public List<Post> Posts { get; set; }

        public User User { get; set; }


        public HomePageModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        public async Task<IActionResult> OnGetAsync(int deletePost)
        {
            Posts = await _modelManagement.UpdateListAsync<Models.Post>();
            if (deletePost != 0)
            {
                await _httpService.HttpDeleteRequest<Models.Post>($"Post/{deletePost}");
                return RedirectToPage();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeletePost(int deletePost)
        {
            if (deletePost != 0)
                await _httpService.HttpDeleteRequest<Models.Post>($"Post/{deletePost}");

            return RedirectToPage();
        }
    }
}
