
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CollectionController : ControllerBase
    {
        private readonly ILeitnerRepository _repo;
        public CollectionController(ILeitnerRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Collection collection)
        {
            if (collection.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            _repo.Add<Collection>(collection);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetCollection", new { collectionId = collection.Id }, collection);

            throw new System.Exception("Creation the collection failed on save");
        }


        [HttpGet("{collectionId}", Name="GetCollection")]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            var collections = await _repo.GetCollection(collectionId);

            return Ok(collections);
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetCollections([FromQuery] CollectionParams collectionParams)
        {
            if (collectionParams.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var collections = await _repo.GetCollectionsAsync(collectionParams);

            return Ok(collections);
        }
    }
}
