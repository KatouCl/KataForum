using System.Collections.Generic;

namespace KataForum.WebApp.Models.Profile
{
    public class ProfileListingViewModel
    {
        public IEnumerable<ProfileViewModel> Profiles { get; set; }
    }
}