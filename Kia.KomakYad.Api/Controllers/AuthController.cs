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
using Kia.KomakYad.Api.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using System.IO;

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
        private readonly IReCaptchaService _reCaptchaHelper;

        public AuthController(IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService,
            IReCaptchaService reCaptchaHelper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _reCaptchaHelper = reCaptchaHelper;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            if (!await _reCaptchaHelper.Validate(userForRegister.ReCaptchaToken))
            {
                return BadRequest("Prove you are not a robot");
            }
            if (await _userManager.FindByEmailAsync(userForRegister.Email) != null)
            {
                return BadRequest("Another account exists with this email address.");
            }
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

        [HttpPost("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized();

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AuthHelper.MemberRole);
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("GetEmailConfirmationToken({email})")]
        [Authorize]
        public async Task<IActionResult> GetEmailConfirmationToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.EmailConfirmed)
            {
                return BadRequest("You already confirmed your email.");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = QueryHelpers.AddQueryString(_config.GetSection("Spa:emailConfirmUrl").Value,
                new Dictionary<string, string>
                {
                    { "username", user.UserName },
                    {"token", token }
                });

            var emailTemplate = System.IO.File.ReadAllText("templates/confirmEmail.html");
            var message = emailTemplate.Replace("{username}", user.FirstName ?? user.UserName).Replace("{confirmationLink}", confirmationLink);
            await _emailService.SendAsync(message, "Confirm your email address", email);

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

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLogin.Password, true);

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

        [HttpPost("RestorePass")]
        public async Task<IActionResult> Restore(RestorePasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return NotFound("No account associated with the email address");

            var emailToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = QueryHelpers.AddQueryString(_config.GetSection("Spa:resetPassUrl").Value,
                new Dictionary<string, string>
                {
                    {"token", emailToken },
                    { "username", user.UserName },
                });
            var emailTemplate = System.IO.File.ReadAllText("templates/resetPass.html");
            var message = emailTemplate.Replace("{username}", user.FirstName ?? user.UserName).Replace("{resetLink}", resetLink);

            await _emailService.SendAsync(message, "Reset Password", model.Email);
            return NoContent();
        }


        [HttpPost("ResetPass")]
        public async Task<IActionResult> ResetPass(ResetPasswordModel resetPasswordModel)
        {
            var user = await _userManager.FindByNameAsync(resetPasswordModel.Username);

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

            if (result.Succeeded)
                return await Login(new UserForLoginDto { Username = resetPasswordModel.Username, Password = resetPasswordModel.Password });

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