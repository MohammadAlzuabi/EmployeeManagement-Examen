using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace EmployeeManagement.Core.Pages
{
    public class PostPageModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        [BindProperty]
        public Post Post { get; set; }

        public List<Post> Posts { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public int UserId { get; set; }

        public PostPageModel(HttpService httpService, ModelManagement modelManagement)
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
     

            var user = await GetUserById(userId); ;
            if (user != null)
            {
                UserId = user.Id;
            }
            Post.UserId=  UserId ;
            if (ModelState.IsValid || Post != null && Post.UserId != null)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.FirstOrDefault();
                    Post.PostImage = await Helper.UploadImageAsync(file);
                }
                await _httpService.HttpPostRequest($"Post", Post);

                StatusMessage = "Du har skapat inlägget";
                return RedirectToPage(new { userId = Post.UserId });

            }
            else
            {
                StatusMessage = "Det uppstod ett fel";
            }
            return Page();
        }

        private async Task<User> GetUserById(int userId)
        {
            return await _httpService.HttpGetRequest<User>($"User/{userId}");
        }
      
    }
}
