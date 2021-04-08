using System.Collections.Generic;

namespace KataForum.WebApp.Models.Post
{
    public class FavesListingViewModel
    {
        public  IEnumerable<PostListingViewModel> Posts { get; set; }
    }
}