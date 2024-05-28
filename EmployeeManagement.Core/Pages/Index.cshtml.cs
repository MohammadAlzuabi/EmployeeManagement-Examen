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


        public async Task OnGetAsync()
        {
            try
            {
                await EnsureEntitiesExistAsync<Role>();
                await EnsureEntitiesExistAsync<User>();
                await EnsureEntitiesExistAsync<Department>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (IsValidInput(UserInput.Email, UserInput.Password))
            {
                var loggedInUser = await GetLoggedInUserAsync(UserInput.Email);

                if (loggedInUser != null && VerifyPassword(UserInput.Password, loggedInUser.Password))
                {
                    UserManagement.SetLoggedInUser(loggedInUser);
                }
                else
                {
                    ErrorMessage = true;
                    return Page();
                }
            }
            return RedirectToPage("/HomePage");
        }

        public IActionResult OnPostLogoutUser()
        {
            UserManagement.LogoutUser();
            return RedirectToPage();
        }

        private async Task CreateRolesAsync() // Skapa roller
        {
            var roleNames = Enum.GetNames(typeof(Enums.Enums.Roles)).ToList();
            foreach (var roleName in roleNames)
            {
                var isSuccessful = await _httpService.HttpPostRequest($"Role", new Role { Name = roleName });
            }
        }

        private async Task<bool> CreateAdminUserAsync() // Skapa admin med lösenord 123
        {
            const string adminEmail = "admin@nykoping.com";
            const string adminPasswordHash = "8089AB6B1C60E94BF3D0564DCD59E2B9E163FCAD163E810CF975A3CDC3D792E76868DFCB6D45356374253D197E55AEF8D839B9ECA8762B17F8331F08C0EF3257"; // 123
            const string adminRoleName = "Admin";

            var role = await GetRoleAsync(adminRoleName);
            if (role == null)
            {
                return false;
            }

            var admin = CreateAdminUser(adminEmail, adminPasswordHash, role.Id);
            var success = await _httpService.HttpPostRequest($"User", admin);

            return success;
        }

        private async Task<bool> CreateDepartmentAsync()
        {
            var departments = new List<Department>
            {
                new Department { Name = "Adminstatör" },
                new Department { Name = "2A" },
                new Department { Name = "2B" },
                new Department { Name = "1A" }
            };

            var tasks = departments.Select(department => _httpService.HttpPostRequest("Department", department)).ToList();

            var results = await Task.WhenAll(tasks);

            return results.All(success => success);
        }

        private async Task EnsureEntitiesExistAsync<TEntity>() where TEntity : class
        {
            var isExisting = await _modelManagement.CheckIfEntityExistsInDBAsync<TEntity>();

            if (!isExisting)
            {
                switch (typeof(TEntity).Name)
                {
                    case nameof(Role):
                        await CreateRolesAsync();
                        break;
                    case nameof(User):
                        await CreateAdminUserAsync();
                        break;
                    case nameof(Department):
                        await CreateDepartmentAsync();
                        break;
                    default:
                        Console.WriteLine($"Entity of type {typeof(TEntity).Name} is not handled.");
                        break;
                }
            }
        }
        private bool IsValidInput(string email, string password)
        {
            return !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password);
        }

        private async Task<User> GetLoggedInUserAsync(string email)
        {
            var users = await _modelManagement.UpdateListAsync<User>();
            return users?.FirstOrDefault(u => u.Email == email);
        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            return _modelManagement.VerifyPassword(inputPassword, storedPasswordHash);
        }
        private async Task<Role> GetRoleAsync(string roleName)
        {
            return await _httpService.HttpGetRequest<Role>($"Role/{roleName}");
        }
        private User CreateAdminUser(string email, string passwordHash, int roleId)
        {
            return new User
            {
                Email = email,
                Password = passwordHash,
                RoleId = roleId
            };
        }
    }
}
