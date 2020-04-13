using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kia.KomakYad.DataAccess
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (userManager.Users.Any())
            {
                return;
            }

            var userData = File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            var roles = new List<Role>
            {
                new Role{ Name = AuthHelper.AdminRole },
                new Role{ Name = AuthHelper.MemberRole },
                new Role{ Name = AuthHelper.ModeratorRole },
                new Role{ Name = AuthHelper.ReporterRole },
            };

            foreach (var role in roles)
            {
                roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }

            foreach (var user in users)
            {
                userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, AuthHelper.MemberRole).GetAwaiter().GetResult();
            }

            var admin = userManager.FindByNameAsync("kiarash").GetAwaiter().GetResult();
            userManager.AddToRolesAsync(admin, new[] { AuthHelper.AdminRole}).GetAwaiter().GetResult();
        }
    }
}
