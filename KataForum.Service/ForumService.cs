using KataForum.Data;
using KataForum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KataForum.Service
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;
        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums.Include(forum => forum.Posts);
        }

        public Forum GetById(int id)
        {
            var forum = _context.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Replies)
                       .ThenInclude(r => r.User)
                .FirstOrDefault();

            return forum;
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetActiveUsers(int forumId)
        {
            var posts = GetById(forumId).Posts;

            if (posts != null || !posts.Any())
            {
                var postUser = posts.Select(p => p.User);
                var replyUsers = posts.SelectMany(p => p.Replies)
                    .Select(u => u.User);
                
                return postUser.Union(replyUsers).Distinct();
            }

            return new List<ApplicationUser>();
        }

        public bool HasRecentPost(int forumId)
        {
            const int houseAgo = 12;
            var window = DateTime.Now.AddHours(-houseAgo);

            return GetById(forumId).Posts.Any(post => post.Created > window);
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }
    }
}
