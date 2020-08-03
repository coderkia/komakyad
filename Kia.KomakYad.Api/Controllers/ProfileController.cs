using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly ILeitnerRepository _repo;
        private readonly IMapper _mapper;

        public ProfileController(ILeitnerRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdate, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating user {id} failed on save");
        }
    }
}