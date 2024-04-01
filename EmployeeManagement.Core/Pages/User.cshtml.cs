using EmployeeManagement.Core.Mangement;
using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static EmployeeManagement.Core.Enums.Enums;

namespace EmployeeManagement.Core.Pages
{
    public class UserModel : PageModel
    {
        private readonly HttpService _httpService;
        private readonly ModelManagement _modelManagement;

        public UserModel(HttpService httpService, ModelManagement modelManagement)
        {
            _httpService = httpService;
            _modelManagement = modelManagement;
        }
        [BindProperty]
        public User EditUser { get; set; }

        public List<Department> Departments { get; set; }
        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _modelManagement.UpdateListAsync<User>();
            Departments = await _modelManagement.UpdateListAsync<Department>();
            Roles = await _modelManagement.UpdateListAsync<Role>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var user = UserManagement.GetLoggedInUser();

            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                // Konvertera filen till en Base64-kodad sträng
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();
                    EditUser.ProfileImg = memoryStream.ToArray();
                }
            }
            EditUser.DepartmentId = user.DepartmentId;
            EditUser.RoleId = user.RoleId;
            EditUser.Password = user.Password;
            if (user != null)
            {
                if (EditUser != null && EditUser.Id != 0)
                {
                    EditUser.Email = EditUser.Email.Trim();
                    await _httpService.HttpUpdateRequest($"User/{EditUser.Id}", EditUser);
                }

            }
            return RedirectToPage();
        }
    }
}
