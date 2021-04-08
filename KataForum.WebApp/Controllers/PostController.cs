using KataForum.Data;
using KataForum.Data.Models;
using KataForum.WebApp.Models.Post;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KataForum.WebApp.Models.Forum;
using KataForum.WebApp.Models.Reply;
using Microsoft.AspNetCore.Authorization;

namespace KataForum.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
        private readonly IApplicationUser _userService;

        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost postService,
            IForum forumService,
            UserManager<ApplicationUser> userManager,
            IApplicationUser userService)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _userService = userService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexViewModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                AuthorImageUrl = post.User.ProfileImageUrl,
                PostContent = post.Content,
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                IsAuthorAdmin = IsAuthorAdmin(post.User)
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Create(int id)
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostViewModel
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                AuthorName = User.Identity.Name
            };

            return View(model);
        }

        public async Task<IActionResult> EditPost(int id)
        {
            var post = _postService.GetById(id);
            
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            if (post.User.Id == user.Id)
            { 
                var model = new EditPostViewModel
                {
                    ForumId = post.Forum.Id,
                    ForumName = post.Forum.Title,
  
                    PostId = post.Id,
                    Content = post.Content,
                    Title = post.Title
                };
  
                return View(model);      
            }
            
            return Content("Вы не можете изменить данный пост так как вы не автор");      
        }

        public async Task<IActionResult> EditPostContent(EditPostViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var post = BuildPostEdit(model, user);
            
            await _postService.EditPostContent(post);

            return RedirectToAction("Index", "Post", new {id = post.Id});
        }

        

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(NewPostViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var post = BuildPost(model, user);

            await _postService.Add(post);
            await _userService.UpdateUserRating(userId, typeof(Post));

            return RedirectToAction("Index", "Post", new {id = post.Id});
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.Delete(id);

            return Redirect("~/Forum/");
        }

        public async Task<IActionResult> MyThreads()
        {
            var userId = _userManager.GetUserId(User);
            var posts = _postService.GetPostsByUser(userId);

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

            var model = new MythreadListingViewModel
            {
                Posts = postListings
            };
            
            return View(model);
        }

        public async Task<IActionResult> Fave()
        {
            var userId = _userManager.GetUserId(User);
            var posts = _postService.GetPostsByFave(userId);
            
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

            var model = new FavesListingViewModel
            {
                Posts = postListings
            };
            
            return View();
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

        private Post BuildPost(NewPostViewModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum
            };
        }
        
        private Post BuildPostEdit(EditPostViewModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Id = model.PostId,
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum
            };
        }

        private IEnumerable<PostReplyViewModel> BuildPostReplies(IEnumerable<PostReply> postReplies)
        {
            return postReplies.Select(reply => new PostReplyViewModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorRating = reply.User.Rating,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                DateCreated = reply.Created,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }

        private bool IsAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user)
                .Result
                .Contains("Admin");
        }
    }
}