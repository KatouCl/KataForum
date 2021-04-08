using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KataForum.Data.Models;

namespace KataForum.Data
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();
        IEnumerable<Post> GetByUserId(string id);
            
        Task SetProfileImage(string id, Uri uri);
        Task UpdateUserRating(string userId, Type type);
    }
}