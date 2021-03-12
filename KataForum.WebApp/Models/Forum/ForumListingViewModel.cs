namespace KataForum.WebApp.Models.Forum
{
    public class ForumListingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl  { get; set; }
        
        public int NumberOfUsers { get; set; }
        public int NumberOfPosts { get; set; }
        public bool HasRecentPost { get; set; }
    }
}
