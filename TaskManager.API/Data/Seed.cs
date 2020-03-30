using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data
{
    public static class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {

                var user = new User { UserName = "admin" };

                var role = new Role { Name = "Admin" };

                userManager.CreateAsync(user, "admin123").Wait();

                roleManager.CreateAsync(role).Wait();

                userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
