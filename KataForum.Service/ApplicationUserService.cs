﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KataForum.Data;
using KataForum.Data.Models;

namespace KataForum.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public ApplicationUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public IEnumerable<Post> GetByUserId(string id)
        {
            throw new NotImplementedException();
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsolutePath;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRating(string userId, Type type)
        {
            var user = GetById(userId);
            user.Rating = CalculateUserRating(type, user.Rating);

            await _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;

            if (type == typeof(Post))
                inc = 1;
            if (type == typeof(PostReply))
                inc = 1;

            return userRating += inc;
        } 
        
        private int CalculateUserRating(Type type, int userRating, bool status)
        {
            var inc = 0;
            if (status == false)
            {
                if (type == typeof(Post))
                    inc = 1;
                if (type == typeof(PostReply))
                    inc = 1;
            }

            return userRating -= inc;
        } 
    }
}