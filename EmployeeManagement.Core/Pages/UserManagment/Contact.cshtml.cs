using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages.UserManagment
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public Message Message { get; set; }
        public void OnGet()
        {
        }
    }
}
