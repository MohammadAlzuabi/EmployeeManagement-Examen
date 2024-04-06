﻿using EmployeeManagement.API.DAL;
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

        public async Task<bool> UpdatetAsync(int id, Message messages)
        {
            var existingMeessage = await _context.Messages.FindAsync(id);
            if (existingMeessage != null)
            {
                _context.Entry(messages).State = EntityState.Modified;
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
