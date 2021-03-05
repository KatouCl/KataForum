using System.Collections.Generic;
using KataForum.WebApp.Models.Post;

namespace KataForum.WebApp.Models.Search
{
    public class SearchResultViewModel
    {
        public  IEnumerable<PostListingViewModel> Posts { get; set; }
        public  string SearchQuery { get; set; }
        public  bool EmptySearchResults { get; set; }
    }
}