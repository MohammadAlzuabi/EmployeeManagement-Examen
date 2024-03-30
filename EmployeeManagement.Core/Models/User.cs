using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Role? Role { get; set; }

        public int RoleId { get; set; }

        public bool IsAdmin()
        {
            return Role != null && (Role.Name == "admin" || Role.Name == "Admin");
        }

    }
}
