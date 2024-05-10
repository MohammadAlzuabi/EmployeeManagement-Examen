using EmployeeManagement.Core.Mangement;
using EmployeeManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class UserTest
    {
        [Fact]
        public void IsAdmin_ReturnsFalse_WhenRoleIsNotAdmin()
        {
            // Arrange
            var user = new User
            {
                Role = new Role { Name = "User" }
            };

            // Act
            var isAdmin = user.IsAdmin();

            // Assert
            Assert.False(isAdmin);
        }
        [Fact]
        public void SetLoggedInUser_SetsCorrectUser()
        {
            // Arrange
            var expectedUser = new User { Name = "Mohammad", Email = "Mohammad@example.com" };

            // Act
            UserManagement.SetLoggedInUser(expectedUser);

            // Assert
            var loggedInUser = UserManagement.GetLoggedInUser();
            Assert.Equal(expectedUser.Name, loggedInUser.Name);
            Assert.Equal(expectedUser.Email, loggedInUser.Email);
        }

        [Fact]
        public void GetLoggedInUser_ReturnsCorrectUser()
        {
            // Arrange
            var expectedUser = new User { Name = "Mohammad", Email = "Mohammad@example.com" };
            UserManagement.SetLoggedInUser(expectedUser);

            // Act
            var loggedInUser = UserManagement.GetLoggedInUser();

            // Assert
            Assert.Equal(expectedUser, loggedInUser);
        }
        [Fact]
        public void LogoutUser_LogsOutUser()
        {
            // Arrange
            var user = new User { Name = "Mohammad", Email = "Mohammad@example.com" };
            UserManagement.SetLoggedInUser(user);

            // Act
            UserManagement.LogoutUser();

            // Assert
            var loggedInUser = UserManagement.GetLoggedInUser();
            Assert.Null(loggedInUser);
        }
    }
}
