using System;
using System.Threading.Tasks;
using KataForum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KataForum.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var userStore = new UserStore<ApplicationUser>(_context);
            
            var user = new ApplicationUser
            {
                UserName = "whatsoncll@gmail.com",
                NormalizedUserName = "WHATSONCLL@GMAIL.COM",
                Email = "WhatsonCll@gmail.com",
                NormalizedEmail = "WHATSONCLL@GMAIL.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            
            var hasher = new PasswordHasher<ApplicationUser>();
            var hasherPassword = hasher.HashPassword(user, "H3shvyrkov90");

            user.PasswordHash = hasherPassword;
            
            var hasAdminRole = _context.Roles.AnyAsync(roles => roles.Name == "Admin").Result;

            if (!hasAdminRole)
            {
                roleStore.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "admin"
                });
            }

            var hasSuperUser = 
                _context.Users.AnyAsync(u => u.NormalizedUserName == u.UserName).Result; //TODO: не создается пользователь 
            //todo: разобраться с этим участком кода
            //if (!hasSuperUser)
            //{
                userStore.CreateAsync(user);
                userStore.AddToRoleAsync(user, "Admin");
            //}

            _context.SaveChangesAsync();
            
            return Task.CompletedTask;
        }
    }
}