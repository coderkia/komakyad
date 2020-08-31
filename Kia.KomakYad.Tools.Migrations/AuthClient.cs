using Kia.KomakYad.Domain.Dtos;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Kia.KomakYad.Tools.Migrations
{
    class AuthClient
    {
        public static UserDetailedDto Register(UserForRegisterDto user)
        {
            if(user.Email == "aalinasab@gmail.com")
            {
                return new UserDetailedDto
                {
                    Id = 1
                };
            }
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5000/api/");
            var body = JsonConvert.SerializeObject(user);
            var response = httpClient.PostAsync("Auth/Register", new StringContent(body, Encoding.UTF8,"application/json")).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to register - " + response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                Console.ResetColor();
                return null;
            }
            var registeredUser = JsonConvert.DeserializeObject<UserDetailedDto>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            return registeredUser;
        }
    }
}
