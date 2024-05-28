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
        public Message Message { get; set; }

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
            if (User == null)
            {
                return NotFound();
            }

            var messages = await GetMessagesForUserAsync(userId);
            foreach (var message in messages)
            {
                var userMessage = await CreateMessageAsync(message);
                if (userMessage != null)
                {
                    Messages.Add(userMessage);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostReadMessage()
        {
            if (Message.Id != null && Message.FromUserId != null && Message.ToUserId != null && !string.IsNullOrEmpty(Message.Content) && Message.IsRead)
            {
                await UpdateMessageAsync(Message);
                return RedirectToPage(new { userId = Message.ToUserId });
            }

            return BadRequest();
        }
        public async Task<IActionResult> OnPostAsync(int userId)
        {
            User = await GetUserById(userId);
            if (User == null)
            {
                return NotFound();
            }

            var roleName = User.IsAdmin() ? "admin" : "user";
            var roleUser = await GetUserByRoleName(roleName, userId);
            if (roleUser != null)
            {
                Message.FromUserId = roleUser.Id;
            }

            Message.SentAt = DateTime.Now;
            if (ModelState.IsValid && Message != null)
            {
                await PostMessageAsync(Message);
                return RedirectToPage(new { userId = Message.FromUserId });
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
        private async Task<CustomMessageModel> CreateMessageAsync(Message message)
        {
            var fromUser = await GetUserById(message.FromUserId);
            var currentUser = await GetUserById(message.ToUserId);

            if (fromUser != null && currentUser != null)
            {
                return new CustomMessageModel
                {
                    Id = message.Id,
                    Title = message.Title,
                    Content = message.Content,
                    SentAt = message.SentAt,
                    IsRead = message.IsRead,
                    FromUser = fromUser,
                    CurrentUser = currentUser,
                };
            }
            return null;
        }

        private async Task UpdateMessageAsync(Message message)
        {
            await _httpService.HttpUpdateRequest($"Message/{message.Id}", message);
        }

        private async Task PostMessageAsync(Message message)
        {
            await _httpService.HttpPostRequest($"Message", message);
        }
        private async Task<List<Message>> GetMessagesForUserAsync(int userId)
        {
            return await _httpService.HttpGetRequest<List<Message>>($"Message/UserId/{userId}");
        }
    }
}
