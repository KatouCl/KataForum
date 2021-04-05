using KataForum.Data;
using KataForum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KataForum.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> GetLatestPosts(int n)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(n);
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var post = GetById(id);
            
            _context.Attach(post);
            _context.Entry(post).State = EntityState.Deleted;
            
            await _context.SaveChangesAsync();
        }

        public async Task EditPostContent(Post post)
        {
            _context.Entry(await _context.Posts.FirstOrDefaultAsync(x => x.Id == post.Id))
                .CurrentValues.SetValues(post);
            await _context.SaveChangesAsync();
        }

        public async Task AddReply(PostReply reply)
        {
            _context.Add(reply);
            await _context.SaveChangesAsync();
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                .First();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery) ? forum.Posts : forum.Posts.Where(post => post.Title.Contains(searchQuery) 
                                       || post.Content.Contains(searchQuery));
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            var normalzed = searchQuery.ToLower();
            
            return GetAll().Where(post 
                => post.Title.ToLower().Contains(normalzed)
                || post.Content.ToLower().Contains(normalzed));
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums.First(forum => forum.Id == id)
                .Posts;
            
        }
    }
}
