using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AuthHelper.AdminPolicy)]
    public class AdminController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly UserManager<User> _userManager;

        public AdminController(IAuthRepository repo, UserManager<User> userManager)
        {
            _authRepository = repo;
            _userManager = userManager;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _authRepository.GetRoles());
        }

        [HttpGet("user/{username}/roles")]
        public async Task<IActionResult> GetUsersRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound($"Username {username} doesn't exist.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("user/{userId}/addRole({roleName})")]
        public async Task<IActionResult> AddRole(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound($"User ID {userId} doesn't exist.");
            }
            await _userManager.AddToRolesAsync(user, new List<string> { roleName });
            return NoContent();
        }

        [HttpPost("user/{userId}/removeRole({roleName})")]
        public async Task<IActionResult> RemoveRole(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound($"User ID {userId} doesn't exist.");
            }
            await _userManager.RemoveFromRolesAsync(user, new List<string> { roleName });
            return NoContent();
        }

        [HttpGet("UsersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles([FromQuery]UserWithRolesParams filters)
        {
            var userWithRoles = await _authRepository.GetUsersWithRoles(filters);

            Response.AddPagination(userWithRoles);
            return Ok(userWithRoles);
        }

        [HttpPut("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.RoleNames ?? new string[] { };
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove the roles.");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpPatch("lockUser({userId})")]
        public async Task<IActionResult> LockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                NotFound();

            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(1000));

            return NoContent();
        }

        [HttpPatch("unlockUser({userId})")]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                NotFound();

            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now);

            return NoContent();
        }


        [HttpPatch("User/{userId}/CardLimit({limit})")]
        public async Task<IActionResult> SetCardLimits(string userId, int limit)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                NotFound("User not found");
            }

            await _adminRepository.SetCardLimit(user, limit);

            return NoContent();

        }
        [HttpPatch("User/{userId}/CollectionLimit({limit})")]
        public async Task<IActionResult> SetCollectionLimits(string userId, int limit)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                NotFound("User not found");
            }

            await _adminRepository.SetCollectionLimit(user, limit);

            return NoContent();

        }
    }
}