using EmployeeManagement.API.DAL;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class PostRepository(MyDbContext context)
    {

        private readonly MyDbContext _context = context;

        public async Task<IEnumerable<Post>> GetAsync() => await _context.Posts.ToListAsync();

        public async Task<Post> GetByIdAsync(int id) => await _context.Posts.FirstOrDefaultAsync(x => x.Id ==id);
        public async Task<bool> CreateAsync(Post post)
        {
            if (post != null)
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(int id, Post post)
        {
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost != null)
            {
                existingPost.Title = post.Title;
                existingPost.Content = post.Content;    
                existingPost.PostImage = post.PostImage;
                existingPost.CreatedAt = DateTime.Now;
                _context.Posts.Update(existingPost);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
