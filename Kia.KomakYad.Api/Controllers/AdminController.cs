﻿using System;
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
        private readonly IAuthRepository _repo;
        private readonly UserManager<User> _userManager;

        public AdminController(IAuthRepository repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _repo.GetRoles());
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
            var userWithRoles = await _repo.GetUsersWithRoles(filters);

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

        [HttpPatch("lockUser({userName})")]
        public async Task<IActionResult> LockUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                NotFound();

            await _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);

            return NoContent();
        }

        [HttpPatch("unlockUser({userName})")]
        public async Task<IActionResult> UnlockUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                NotFound();

            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now);

            return NoContent();
        }
    }
}