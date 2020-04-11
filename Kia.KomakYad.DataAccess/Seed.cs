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

        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.Users.Any())
            {
                return;
            }

            var userData = File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                userManager.CreateAsync(user, "password").GetAwaiter();
            }
        }
    }
}
