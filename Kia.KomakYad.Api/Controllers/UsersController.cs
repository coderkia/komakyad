using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthHelper.AdminPolicy)]
    public class UsersController : ControllerBase
    {
        private readonly ILeitnerRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(ILeitnerRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            userParams.UserId = currentUserId;

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserDetailedDto>(user);

            return Ok(userToReturn);
        }
    }
}