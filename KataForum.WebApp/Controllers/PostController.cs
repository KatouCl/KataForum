using KataForum.Data;
using KataForum.Data.Models;
using KataForum.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KataForum.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;
        public PostController(IPost postService)
        {
            _postService = postService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.PostReplies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies
            };

            return View(model);
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> postReplies)
        {
            return postReplies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorId = reply.User.Id,
                AuthorName = reply.User.UserName,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content
                //PostId = reply.Post.Id
            }) ;
        }
    }
}
