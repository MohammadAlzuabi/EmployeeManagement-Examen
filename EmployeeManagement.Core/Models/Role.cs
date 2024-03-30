using System.ComponentModel;

namespace EmployeeManagement.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
