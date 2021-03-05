using System.Linq;
using ASP;
using KataForum.Data;
using KataForum.Data.Models;
using KataForum.WebApp.Models.Forum;
using KataForum.WebApp.Models.Post;
using KataForum.WebApp.Models.Search;
using Microsoft.AspNetCore.Mvc;

namespace KataForum.WebApp.Controllers
{
    public class SearchController : Controller
    {
        
        private readonly IPost _postService;
        
        public SearchController (IPost postService)
        {
            _postService = postService;
        }

        // GET
        public IActionResult Result(string searchQuery)
        {
            var posts = _postService.GetFilteredPosts(searchQuery);

            var noResult = 
                (!string.IsNullOrEmpty(searchQuery) && posts.Any());

            var postListings = posts.Select(post => new PostListingViewModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
                    
            });

            var model = new SearchResultViewModel
            {
                Posts = postListings,
                SearchQuery = searchQuery,
                EmptySearchResults = noResult
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Result", new {searchQuery});
        }
        
        private ForumListingViewModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            
            return new ForumListingViewModel
            {
                Id = forum.Id,
                ImageUrl = forum.ImageUrl,
                Name = forum.Title,
                Description = forum.Description
            };
        }
    }
}