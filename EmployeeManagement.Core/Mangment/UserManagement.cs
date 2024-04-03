using EmployeeManagement.Core.Models;

namespace EmployeeManagement.Core.Mangement
{
    public class UserManagement
    {
        private static Models.User? _loggedInUser = new Models.User();

        public static void SetLoggedInUser(Models.User user)
        {
            _loggedInUser = user;
        }
        public static void UpdateLoggedInUsers(User loggedInUsers)
        {
            // Uppdatera den statiska listan med inloggade användare
            _loggedInUser = loggedInUsers;
        }
        public static Models.User GetLoggedInUser()
        {
            return _loggedInUser;
        }
        public static void LogoutUser()
        {
            if (_loggedInUser is null)
                return;
            _loggedInUser = null;
        }
    }
}
