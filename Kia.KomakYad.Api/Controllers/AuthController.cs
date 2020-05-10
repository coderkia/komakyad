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
using Kia.KomakYad.Api.Models;
using Kia.KomakYad.Common.Services;

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
        private readonly EmailService _emailService;

        public AuthController(IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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

            await GetEmailConfirmationToken(userToCreate.Email);

            return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
        }

        [HttpGet("ConfirmEmail/{userId}/token/{token}", Name ="ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userName, string token)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Unauthorized();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return NoContent();

            return BadRequest(result.Errors);
        }

        [HttpPost("GetEmailConfirmationToken({email})")]
        [Authorize]
        public async Task<IActionResult> GetEmailConfirmationToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Unauthorized();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"{Request.Scheme}://{Request.Host}" + Url.Action("ConfirmEmail", new { token, userName = user.UserName });

            var message = $"Hi {user.FirstName}\r\n Confirm your email address by clicking on following link.\r\n{confirmationLink}";
            await _emailService.SendAsync(message, "Confirm your email address", email, false);

            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var user = await _userManager.FindByNameAsync(userForLogin.Username);

            if (user == null)
            {
                return Unauthorized("Username or Password is wrong.");
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

        [HttpPost("RestorePass({userName})")]
        public async Task<IActionResult> Restore(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            var emailToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            //todo send email

            return NoContent();
        }

        [HttpPost("ResetPass({token})")]
        public async Task<IActionResult> ResetPass(string token, ResetPasswordModel resetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordModel.NewPassword);

            if (result.Succeeded)
                return NoContent();

            return BadRequest(result.Errors);
        }

        [HttpPost("ChangePass")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);

            if (result.Succeeded)
                return NoContent();

            return BadRequest(result.Errors);

        }

        private async Task<string> GenetrateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
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