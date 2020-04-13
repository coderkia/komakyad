using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            var userToCreate = _mapper.Map<User>(userForRegister);

            var result = await _userManager.CreateAsync(userToCreate, userForRegister.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var userToReturn = _mapper.Map<UserDetailedDto>(userToCreate);

            return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var user = await _userManager.FindByNameAsync(userForLogin.Username);

            if(user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLogin.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var userDto = _mapper.Map<UserListDto>(user);
            return Ok(new
            {
                token = await GenetrateToken(user),
                user = userDto
            });
        }

        private async Task<string> GenetrateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}