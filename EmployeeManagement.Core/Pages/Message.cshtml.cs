using EmployeeManagement.Core.Mangement;
using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages
{
    public class MessageModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        [TempData]
        public string StatusMessage { get; set; }


        public List<Models.User> Users { get; set; }
        [BindProperty]
        public Message SendMessage { get; set; }
        public int UserId { get; set; }
        public User CreatedByUser { get; set; }

        public MessageModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        public async Task<IActionResult> OnGetAsync(int userId)
        {
            var user = await _httpService.HttpGetRequest<Models.User>($"User/{userId}");
            CreatedByUser = await _httpService.HttpGetRequest<Models.User>($"User/{1}");
            Users = await _modelManagement.UpdateListAsync<Models.User>();

            if (userId != 0)
            {
                UserId = userId;
            }
            if (user == null || CreatedByUser == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostSendMessage(int userId)
        {
            var user = UserManagement.GetLoggedInUser();
            CreatedByUser = await _httpService.HttpGetRequest<Models.User>($"User/{1}");

            if (user != null)
            {
                UserId = user.Id;
            }
            SendMessage.SentAt = DateTime.Now;
            SendMessage.ToUserId = CreatedByUser.Id;
            SendMessage.FromUserId = UserId;
            if (SendMessage.Id != 0 && SendMessage.FromUserId != 0 && SendMessage.ToUserId != 0 && (SendMessage.Content != null || SendMessage.Content != string.Empty))
            {
                await _httpService.HttpPostRequest($"Message", SendMessage);
                return RedirectToPage("./Index");

            }
            StatusMessage = "Failed to send message!";
            return Page();
        }
    }

}
