using EmployeeManagement.API.DAL;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class MessageRepository
    {
        private MyDbContext _context;
        public MessageRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Message>> GetAsync() => await _context.Messages.ToListAsync();

        public async Task<Message> GetOneAsync(int id) => await _context.Messages.FirstOrDefaultAsync();

        public async Task<List<Message>> GetAllById(int id) => await _context.Messages.Where(m => m.ToUserId == id ).ToListAsync();

        public async Task<bool> CreateAsync(Message messages)
        {
            if (messages != null)
            {
                _context.Messages.Add(messages);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatetAsync(int id, Message message)
        {
            var existingMessage = await _context.Messages.FindAsync(id);
            if (existingMessage != null)
            {
                existingMessage.Id = message.Id;
                existingMessage.Title = message.Title;
                existingMessage.Content = message.Content;
                existingMessage.ToUserId = message.ToUserId;
                existingMessage.FromUserId = message.FromUserId;
                existingMessage.SentAt = message.SentAt;
                existingMessage.IsRead = message.IsRead;

                _context.Messages.Update(existingMessage);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var meesage = await _context.Messages.FindAsync(id);
            if (meesage != null)
            {
                _context.Messages.Remove(meesage);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



    }
}
