using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KataForum.WebApp.Models.Forum
{
    public class ForumIndexModel
    {
        public IEnumerable<ForumListingViewModel> ForumList { get; set; }
    }
}
