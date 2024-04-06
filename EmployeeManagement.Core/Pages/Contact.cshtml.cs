using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages.UserManagment
{
    public class ContactModel : PageModel
    {
        private readonly ModelManagement _modelManagement;
        private readonly HttpService _httpService;
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public Message ReplyMessage { get; set; }

        public List<Message> Messages { get; set; }

        public ContactModel(ModelManagement modelManagement, HttpService httpService)
        {
            _modelManagement = modelManagement;
            _httpService = httpService;
        }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            var user = await _httpService.HttpGetRequest<Models.User>($"User/{userId}"); ;
            if (user == null) return NotFound();

            Messages = await _httpService.HttpGetRequest<List<Message>>($"Message/UserId/{userId}");
           
            return Page();
        }

        public async Task<IActionResult> OnPostReplyMessage()
        {

            ReplyMessage.SentAt = DateTime.Now;
            if (ModelState.IsValid || ReplyMessage != null)
            {
                await _httpService.HttpPostRequest($"Message", ReplyMessage);
                return RedirectToPage(new { userId = ReplyMessage.FromUser });

            }

            return BadRequest();
        }
    }
}
