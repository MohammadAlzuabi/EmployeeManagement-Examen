using EmployeeManagement.Core.Mangement;
using EmployeeManagement.Core.Mangment;
using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
        public Models.User EditUser { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public List<Department> Departments { get; set; }
        public List<Models.User> Users { get; set; }
        public List<Role> Roles { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _modelManagement.UpdateListAsync<Models.User>();
            Departments = await _modelManagement.UpdateListAsync<Department>();
            Roles = await _modelManagement.UpdateListAsync<Role>();

     
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await loadIamge();
            var user = UserManagement.GetLoggedInUser();

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
            UserManagement.UpdateLoggedInUsers(EditUser);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task loadIamge()
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();
                await using (var dataStream = new MemoryStream())
                {
                    if (file != null)
                    {
                        await file.CopyToAsync(dataStream);
                        EditUser.ProfileImg = dataStream.ToArray();
                    }

                }
            }
            UserManagement.UpdateLoggedInUsers(EditUser);
        }
    }
}
