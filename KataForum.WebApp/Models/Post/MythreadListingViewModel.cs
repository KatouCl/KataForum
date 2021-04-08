using System;
using System.Collections.Generic;

namespace KataForum.WebApp.Models.Post
{
    public class MythreadListingViewModel
    {
        public  IEnumerable<PostListingViewModel> Posts { get; set; }
    }
}