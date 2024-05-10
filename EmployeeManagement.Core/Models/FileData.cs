namespace EmployeeManagement.Core.Models
{
    public class FileData
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public byte[] File { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
