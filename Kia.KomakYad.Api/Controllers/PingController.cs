using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet("/ping")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new
            {
                version = typeof(PingController).Assembly.GetName().Version.ToString()
            });
        }
    }
}