using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages
{
    public class AdminModel : PageModel
    {

        [BindProperty]
        public List<Models.User> Users { get; set; }

        [BindProperty]
        public List<Role> Roles { get; set; }

        [BindProperty]
        public List<Department> Departments { get; set; }

        [BindProperty]
        public Models.User EditUser { get; set; }

        [BindProperty]
        public Models.User NewUser { get; set; }

        public string? SearchString { get; set; }

        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        public AdminModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        public async Task<IActionResult> OnGetAsync(string searchString)
        {
            Roles = await _modelManagement.UpdateListAsync<Role>();
            Users = await _modelManagement.UpdateListAsync<Models.User>();
            Departments = await _modelManagement.UpdateListAsync<Department>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredUsers = Users.Where(p =>
                    p.Email.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Role.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                Users = filteredUsers;
            }
      

            return Page();

        }

        public async Task<IActionResult> OnPostDeleteUser([FromForm] int deleteId)
        {
            if (deleteId != 0)
            {
                await DeleteUserAsync(deleteId);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostNewUser()
        {

            if (NewUser != null && !string.IsNullOrWhiteSpace(NewUser.Email))
            {
                NewUser.Email = NewUser.Email.Trim();
                NewUser.Password = _modelManagement.HashPassword(NewUser.Password);
                await CreateUserAsync(NewUser);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditUser()
        {
            if (EditUser != null && EditUser.Id != 0)
            {
                EditUser.Email = EditUser.Email.Trim();
                await UpdateUserAsync(EditUser);
            }
            return RedirectToPage();
        }

        private async Task DeleteUserAsync(int userId)
        {
            await _httpService.HttpDeleteRequest<Models.User>($"User/{userId}");
        }

        private async Task CreateUserAsync(User newUser)
        {
            await _httpService.HttpPostRequest($"User", newUser);
        }

        private async Task UpdateUserAsync(User editUser)
        {
            await _httpService.HttpUpdateRequest($"User/{editUser.Id}", editUser);
        }
    }
}
