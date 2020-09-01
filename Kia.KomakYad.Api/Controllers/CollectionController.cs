using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
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
    [Authorize(Policy = AuthHelper.ReadPolicy)]
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
        public async Task<IActionResult> Create(CollectionCreateDto collectionToCreate)
        {
            if (collectionToCreate.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (int.TryParse(User.FindFirst(CustomClaimTypes.CollectionLimit).Value, out int collectionLimit))
            {
                if (await _repo.GetCollectionsCount(collectionToCreate.AuthorId) > collectionLimit)
                {
                    return BadRequest($"You cannot add more than {collectionLimit} collections.");
                }
            }

            var collection = _mapper.Map<Collection>(collectionToCreate);

            _repo.Add(collection);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetCollection", new { collectionId = collection.Id }, collection);

            throw new System.Exception("Creation the collection failed on save");
        }

        [HttpPut("{collectionId}")]
        public async Task<IActionResult> Update(int collectionId, CollectionToUpdateDto collectionToUpdate)
        {
            var collection = await _repo.GetCollection(collectionId);

            if (collection.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            collectionToUpdate.Map(collection);

            _repo.Update(collection);

            if (await _repo.SaveAll())
                return NoContent();

            throw new System.Exception($"Updating the collection {collection.Id}  failed on save");
        }

        [HttpPatch("{collectionId}/policy/{policy}")]
        public async Task<IActionResult> ChangeAccessPolicy(int collectionId, string policy)
        {
            var collection = await _repo.GetCollection(collectionId);
            if (collection == null)
                return NotFound();

            if (collection.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            switch (policy.ToLower())
            {
                case "private":
                    collection.IsPrivate = true;
                    break;
                case "public":
                    collection.IsPrivate = false;
                    break;
                default:
                    return BadRequest("Unkown Policy");
            }

            if (await _repo.SaveAll())
                return NoContent();

            throw new System.Exception($"Changing policy of the collection {collection.Id} failed on save");
        }

        [HttpGet("{collectionId}", Name = "GetCollection")]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            var collection = await _repo.GetCollection(collectionId);
            if (collection == null)
            {
                return BadRequest();
            }
            var collectionToReturn = _mapper.Map<CollectionToReturnDto>(collection);
            collectionToReturn.CardsCount = await _repo.GetCardsCount(collection.Id);
            collectionToReturn.FollowersCount = await _repo.GetFollowersCount(collection.Id);
            return Ok(collectionToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetCollections([FromQuery] CollectionParams collectionParams)
        {
            collectionParams.IncludePrivateCollections = true;
            if (!User.IsInRole(AuthHelper.AdminRole))
            {
                if (collectionParams.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    collectionParams.IncludePrivateCollections = false;
            }

            var collections = await _repo.GetCollections(collectionParams);

            var collectionToReturns = _mapper.Map<IEnumerable<CollectionToReturnDto>>(collections);

            foreach (var collection in collectionToReturns)
            {
                collection.CardsCount = await _repo.GetCardsCount(collection.Id);
                collection.FollowersCount = await _repo.GetFollowersCount(collection.Id);
            }
            Response.AddPagination(collections);

            return Ok(collectionToReturns);
        }
    }
}
