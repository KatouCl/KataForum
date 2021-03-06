﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KataForum.Data.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public int Rating { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime MemberSince { get; set; }
        public bool isActive { get; set; }
        
        public virtual IEnumerable<Post> Faves { get; set; }
    }
}
