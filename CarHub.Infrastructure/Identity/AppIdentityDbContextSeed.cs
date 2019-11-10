using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHub.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        private const string AdminUser = "admin@carhub.com";

        public static async Task SeedAsync(UserManager<IdentityUser> userManager)
        {
            if ((await userManager.FindByEmailAsync("admin@carhub.com")) == null)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = AdminUser,
                    UserName = AdminUser
                };

                await userManager.CreateAsync(user, "Test1234!");
            }
        }
    }
}
