using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Core.Pages.User
{
    [Authorize]
    public class MyMessagesModel : PageModel
    {
     
        private readonly Models.User _user;

        public MyMessagesModel()
        {
            
        }
    }
}
