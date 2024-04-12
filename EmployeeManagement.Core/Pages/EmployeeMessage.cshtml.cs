using EmployeeManagement.Core.Mangement;
using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages
{
    public class EmployeeMessageModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public Models.Message SendMessage { get; set; }
        public int UserId { get; set; }
        [BindProperty]
        public User SendToUser { get; set; }

        public EmployeeMessageModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            await SetUserData(userId);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int userId)
        {
            await SetUserData(userId);
            var user = UserManagement.GetLoggedInUser();

            if (user != null)
            {
                UserId = user.Id;
            }
            SendMessage.SentAt = DateTime.Now;
            SendMessage.ToUserId = SendToUser.Id;
            SendMessage.FromUserId = UserId;
            if (SendMessage.FromUserId != 0 && SendMessage.ToUserId != 0 && (SendMessage.Content != null || SendMessage.Content != string.Empty))
            {
                await _httpService.HttpPostRequest($"Message", SendMessage);
                return RedirectToPage("./EmployeeMessage");

            }
            StatusMessage = "Failed to send message!";
            return Page();
        }



        private async Task SetUserData(int userId)
        {
            var user = await _httpService.HttpGetRequest<Models.User>($"User/{userId}");
            SendToUser = await _httpService.HttpGetRequest<Models.User>($"User/rolename/admin");  

            if (userId != 0)
            {
                UserId = userId;
            }
            if (user == null || SendToUser == null)
            {
                throw new InvalidOperationException("User or CreatedByUser not found");
            }
        }
    }

}
