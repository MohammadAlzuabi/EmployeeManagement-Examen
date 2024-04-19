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



        public class CustomPostModel
        {
            public string Id { get; set; }

            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public HomePageModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        public async Task<IActionResult> OnGetAsync(int postId,int userId)
        {
            Posts = await _modelManagement.UpdateListAsync<Models.Post>();
            return Page();
        }
    }
}
