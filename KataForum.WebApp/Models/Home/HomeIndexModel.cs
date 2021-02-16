using System.Collections.Generic;
using KataForum.WebApp.Models.Post;

namespace KataForum.WebApp.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
        
    }
}