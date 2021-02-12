using KataForum.WebApp.Models.Post;
using System.Collections.Generic;

namespace KataForum.WebApp.Models.Forum
{
    public class ForumTopicModel
    {
        public ForumListingModel Forum { get; set; }
        public IEnumerable<PostListingModel> Posts { get; set; }
    }
}
