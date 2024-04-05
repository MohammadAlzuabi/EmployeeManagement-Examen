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
        public Models.User EditUser { get; set; }

        public int UserId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public List<Department> Departments { get; set; }
        public List<Models.User> Users { get; set; }
        public List<Role> Roles { get; set; }


        public async Task<IActionResult> OnGetAsync(int userId)
        {
            EditUser = await _httpService.HttpGetRequest<Models.User>($"User/{userId}");
            Roles = await _modelManagement.UpdateListAsync<Role>();
            Departments = await _modelManagement.UpdateListAsync<Department>();
            if (userId != 0)
            {
                EditUser.Id = userId;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int userId)
        {
            await loadIamge();

            var user = await _httpService.HttpGetRequest<Models.User>($"User/{userId}");
            EditUser.Id = userId;
            if (EditUser != null && EditUser.Id != 0)
            {
                await _httpService.HttpUpdateRequest($"User/{EditUser.Id}", EditUser);
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToPage($"/User", new {userId});
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
        }
    }
}
