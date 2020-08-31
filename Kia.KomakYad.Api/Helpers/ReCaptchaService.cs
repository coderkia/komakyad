using Kia.KomakYad.Api.Configs;
using Kia.KomakYad.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Helpers
{
    public class ReCaptchaService: IReCaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly ReCaptchaConfig _config;

        public ReCaptchaService(IHttpClientFactory httpClientFactory, IOptions<ReCaptchaConfig> config)
        {
            _httpClient = httpClientFactory.CreateClient("IReCaptchaService");
            _config = config.Value;
        }
        public async Task<bool> Validate(string token)
        {
            if (!_config.Enabled)
                return true;
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.google.com/recaptcha/api/siteverify?secret={ _config.SecretKey}&response={token}");

            var response =  await (await _httpClient.SendAsync(request)).Content.ReadAsStringAsync();

            var recaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(response);
            if (recaptchaResponse.Success)
            {
                return true;
            }
            Log.Warning("Error from reCaptcha " + response);
            return false;
        }
    }
}
