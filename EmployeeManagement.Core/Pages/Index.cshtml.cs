using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Enums;
using EmployeeManagement.Core.Mangement;
namespace EmployeeManagement.Core.Pages
{
    public class IndexModel : PageModel
    {
        public List<Models.User?> Users { get; set; }

        [BindProperty]
        public Models.User UserInput { get; set; }
        public Models.User? LoggedInUser { get; set; }

        public bool ErrorMessage { get; set; }

        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        public IndexModel(HttpService httpService,
    ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }




        public async Task<IActionResult> OnPostAsync()
        {
            if (UserInput.Email != null && UserInput.Password != null)
            {
                Users = await _modelManagement.UpdateListAsync<Models.User>();
                if (Users is null || !Users.Any())
                    return RedirectToPage();

                LoggedInUser = Users.FirstOrDefault(u => u.Email == UserInput.Email);
                if (LoggedInUser is null)
                    return RedirectToPage();

                UserManagement.SetLoggedInUser(LoggedInUser);

            }
            return RedirectToPage();
        }

        public IActionResult OnPostLogoutUser()
        {
            UserManagement.LogoutUser();

            return RedirectToPage();
        }
    }
}
