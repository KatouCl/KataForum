﻿using KataForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KataForum.Data
{
    public interface IPost
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery);
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);
        IEnumerable<Post> GetLatestPosts(int n);
        IEnumerable<Post> GetPostsByUser(string id);
        IEnumerable<Post> GetPostsByFave(string id);

        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(Post post);
        Task AddReply(PostReply reply);
    }
}
