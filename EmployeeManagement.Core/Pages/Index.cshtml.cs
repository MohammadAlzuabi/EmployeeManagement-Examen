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



        public async void OnGetAsync()
        {
            var isExisting = await _modelManagement.CheckIfEntityExistsInDBAsync<Role>();
            if (isExisting is false)
            {
                await CreateRolesAsync();
            }
            isExisting = await _modelManagement.CheckIfEntityExistsInDBAsync<User>();
            if (isExisting is false)
            {
                await CreateAdminUserAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UserInput.Email != null && UserInput.Password != null)
            {
                Users = await _modelManagement.UpdateListAsync<User>();
                if (Users is null || !Users.Any())
                    return RedirectToPage();

                LoggedInUser = Users.FirstOrDefault(u => u.Email == UserInput.Email);
                if (LoggedInUser is null)
                    return RedirectToPage();

                var isValideted = _modelManagement.VerifyPassword(UserInput.Password, LoggedInUser.Password);
                if (isValideted)
                {
                    UserManagement.SetLoggedInUser(LoggedInUser);
                }
                else
                {
                    ErrorMessage = true;
                    return Page();
                }
            }
            return RedirectToPage();
        }

        public IActionResult OnPostLogoutUser()
        {
            UserManagement.LogoutUser();
            return RedirectToPage();
        }

        private async Task CreateRolesAsync()
        {
            var roleNames = Enum.GetNames(typeof(Enums.Enums.Roles)).ToList();
            foreach (var roleName in roleNames)
            {
                var isSuccessful = await _httpService.HttpPostRequest($"Role", new Role { Name = roleName });
            }
        }

        private async Task<bool> CreateAdminUserAsync()
        {
            var adminRoleName = Enums.Enums.Roles.Admin.ToString();
            var role = await _httpService.HttpGetRequest<Role>($"Role/{adminRoleName}");
            var admin = new User()
            {
                Email = "admin@nykoping.com",
                Password = "8089AB6B1C60E94BF3D0564DCD59E2B9E163FCAD163E810CF975A3CDC3D792E76868DFCB6D45356374253D197E55AEF8D839B9ECA8762B17F8331F08C0EF3257", //123
                RoleId = role.Id
            };
            return await _httpService.HttpPostRequest($"User", admin);
        }
    }
}
