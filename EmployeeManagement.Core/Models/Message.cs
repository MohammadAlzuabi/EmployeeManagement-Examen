using System.Text.Json.Serialization;

namespace EmployeeManagement.Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ToUserId { get; set; }
        public virtual User? ToUser { get; set; }
        public int FromUserId { get; set; }
        public virtual User? FromUser { get; set; }
        public string Content  { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
