using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KataForum.Data.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public int Rating { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime MemberSince { get; set; }
        public bool isActive { get; set; }
    }
}
