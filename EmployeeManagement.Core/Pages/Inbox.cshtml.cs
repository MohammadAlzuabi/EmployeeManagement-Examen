using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages.UserManagment
{
    public class InboxModel : PageModel
    {
        private readonly ModelManagement _modelManagement;
        private readonly HttpService _httpService;
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public Message ReplyMessage { get; set; }

        //public List<Message> Messages { get; set; }


        public User User { get; set; }

        public List<CustomMessageModel> Messages { get; set; }
        public InboxModel(ModelManagement modelManagement, HttpService httpService)
        {
            _modelManagement = modelManagement;
            _httpService = httpService;
            Messages = new List<CustomMessageModel>();
        }
        public class CustomMessageModel
        {
            public int Id { get; set; }
            public User FromUser { get; set; }
            public User CurrentUser { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime SentAt { get; set; }
            public bool IsRead { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int userId)
        {

            User = await GetUserById(userId);

            var messages = await _httpService.HttpGetRequest<List<Message>>($"Message/UserId/{userId}");
            foreach (var message in messages)
            {
                var fromUser = await GetUserById(message.FromUserId);
                var currentUser = await GetUserById(message.ToUserId);

                if (fromUser != null && currentUser != null)
                {
                    var customMessage = new CustomMessageModel
                    {
                        Id = message.Id,
                        Title = message.Title,
                        Content = message.Content,
                        SentAt = message.SentAt,
                        IsRead = message.IsRead,
                        FromUser = fromUser,
                        CurrentUser = currentUser,
                    };
                    Messages.Add(customMessage);
                }
            }
                return Page();
        }

        public async Task<IActionResult> OnPostAsync(int userId)
        {
            User = await GetUserById(userId);
            var roleName = User.IsAdmin() ? "admin" : "user";
            var roleUser = await GetUserByRoleName(roleName, userId);
            if (roleUser != null)
            {
                ReplyMessage.FromUserId = roleUser.Id;
            }

            ReplyMessage.SentAt = DateTime.Now;
            if (ModelState.IsValid || ReplyMessage != null)
            {
                await _httpService.HttpPostRequest($"Message", ReplyMessage);
                return RedirectToPage(new { userId = ReplyMessage.FromUserId });

            }

            return BadRequest();
        }
        private async Task<User> GetUserById(int userId)
        {
            return await _httpService.HttpGetRequest<User>($"User/{userId}");
        }
        private async Task<User> GetUserByRoleName(string roleName, int userId)
        {
            return await _httpService.HttpGetRequest<User>($"User/roleName/{roleName}/userId/{userId}");
        }
    }
}
