using System.Collections.Generic;
using KataForum.WebApp.Models.Post;

namespace KataForum.WebApp.Models.Home
{
    public class HomeIndexViewModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostIndexViewModel> LatestPosts { get; set; }
        
    }
}