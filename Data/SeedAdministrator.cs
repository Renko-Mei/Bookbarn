using BookBarn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBarn.Data
{
    public static class SeedAdministrator
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // User Info
            string userName = "SuperAdmin";
            string email = "superadmin@gmail.com";
            string password = "Piranha$94";
            string role = "SuperAdministrator";

            if (await userManager.FindByNameAsync(userName) == null)
            {
                // Create SuperAdmins role if it doesn't exist
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Create user account if it doesn't exist
                User user = new User
                {
                    UserName = userName,
                    Email = email
                };

                IdentityResult result = await userManager.CreateAsync(user, password);

                // Assign role to the user
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
