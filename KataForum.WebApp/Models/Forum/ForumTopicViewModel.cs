using KataForum.WebApp.Models.Post;
using System.Collections.Generic;

namespace KataForum.WebApp.Models.Forum
{
    public class ForumTopicViewModel
    {
        public ForumListingViewModel Forum { get; set; }
        public IEnumerable<PostListingViewModel> Posts { get; set; }
    }
}
