

using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;
        public CollectionController(ILeitnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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


        [HttpGet("{collectionId}", Name = "GetCollection")]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            var collections = await _repo.GetCollection(collectionId);

            return Ok(collections);
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetCollections([FromQuery] CollectionParams collectionParams)
        {
            if (collectionParams.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var collections = await _repo.GetCollections(collectionParams);

            return Ok(collections);
        }

        [HttpGet("{collectionId}/Cards", Name = "GetCards")]
        public async Task<IActionResult> GetCards(int collectionId, [FromQuery]CardParams cardParams)
        {
            cardParams.CollectionId = collectionId;
            var cards = await _repo.GetCards(cardParams);
            if (cards == null)
            {
                return BadRequest();
            }
            var cardToReturnDtos = _mapper.Map<IEnumerable<CardToReturnDto>>(cards);

            Response.AddPagination(cards.CurrentPage, cards.PageSize, cards.TotalCount, cards.TotalPages);

            return Ok(cardToReturnDtos);
        }
    }
}
