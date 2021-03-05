using KataForum.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using KataForum.Data;
using KataForum.Data.Models;
using KataForum.WebApp.Models.Forum;
using KataForum.WebApp.Models.Home;
using KataForum.WebApp.Models.Post;

namespace KataForum.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPost _postService;

        public HomeController(ILogger<HomeController> logger, IPost postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View();// model
        }

        private HomeIndexViewModel BuildHomeIndexModel()
        {
            /*var latestPosts = _postService.GetLatestPosts(10);
            
            var posts = latestPosts.Select(post => new PostListingViewModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                
                Forum = GetForumListingForPost(post)
            });*/

            return new HomeIndexViewModel
            {
                //LatestPosts = posts,
                SearchQuery = "" //TODO: search posts
            };
        }

        private ForumListingViewModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingViewModel
            {
                Id = forum.Id,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
