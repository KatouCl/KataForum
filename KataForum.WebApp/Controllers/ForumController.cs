using System;
using System.Collections.Generic;
using KataForum.Data;
using KataForum.Data.Models;
using KataForum.WebApp.Models.Forum;
using KataForum.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KataForum.WebApp.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        public ForumController(IForum forumService, IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingViewModel{
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
                });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
            var posts = new List<Post>();
            
            posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();
            
            var postListings = posts.Select(post => new PostListingViewModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            var model = new ForumTopicViewModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new {id, searchQuery});
        }
        private ForumListingViewModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
        private ForumListingViewModel BuildForumListing(Forum forum)
        {
            return new ForumListingViewModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl 
            };
        }
    }
}
