using System;
using System.Threading.Tasks;
using KataForum.Data;
using KataForum.Data.Models;
using KataForum.WebApp.Models.Reply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KataForum.WebApp.Controllers
{
    [Authorize]
    public class ReplyController : Controller
    {
        private readonly IPost _postService;
        private readonly IApplicationUser _userService;
        
        private static UserManager<ApplicationUser> _userManager;

        public ReplyController(IPost postService, UserManager<ApplicationUser> userManager, IApplicationUser userService)
        {
            _postService = postService;
            _userManager = userManager;
            _userService = userService;
        }
        
        // GET
        public async Task<IActionResult> Create(int id)
        {
            var post = _postService.GetById(id);
            
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var model = new PostReplyViewModel
            {
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,
                
                AuthorId = user.Id,
                AuthorName = User.Identity.Name,
                AuthorRating = user.Rating,
                IsAuthorAdmin = User.IsInRole("Admin"),
                
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                
                DateCreated = DateTime.Now
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            
            var reply = BuildReply(model, user); 

            await _postService.AddReply(reply);
            await _userService.UpdateUserRating(userId, typeof(PostReply));

            return RedirectToAction("Index", "Post", new {id = model.PostId});
        }

        private PostReply BuildReply(PostReplyViewModel model, ApplicationUser user)
        {
            var post = _postService.GetById(model.PostId);

            return new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                Created = DateTime.Now,
                User = user
            };
        }
    }
}