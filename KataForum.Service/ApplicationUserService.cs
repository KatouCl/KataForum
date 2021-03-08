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

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsolutePath;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public Task IncrementRating(string id, Type type)
        {
            throw new NotImplementedException();
        }
    }
}