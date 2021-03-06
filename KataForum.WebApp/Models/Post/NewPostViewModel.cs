﻿using System.ComponentModel.DataAnnotations;

namespace KataForum.WebApp.Models.Post
{
    public class NewPostViewModel
    {
        public string ForumName { get; set; }
        public int ForumId { get; set; }
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "Заполните название поста")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Заполните контент поста")]
        public string Content { get; set; } 
    }
}