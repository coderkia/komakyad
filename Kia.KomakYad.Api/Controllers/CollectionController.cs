using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = AuthHelper.ReadPolicy)]
    public class CollectionController : KomakYadBaseController
    {
        private readonly ICollectionRespository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CollectionController(ICollectionRespository repo, IMapper mapper, UserManager<User> userManager)
        {
            _repository = repo;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CollectionCreateDto collectionToCreate)
        {
            if (collectionToCreate.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(collectionToCreate.AuthorId.ToString());
            if (!user.EmailConfirmed)
            {
                return StatusCode(403, "Confirm your eamil first");
            }

            if (user.CollectionLimit.HasValue)
            {
                var usersTotalCollectionCount = await _repository.GetUsersCollectionsCount(user.Id);
                if (usersTotalCollectionCount > user.CollectionLimit)
                {
                    return BadRequest($"You cannot add more than {user.CollectionLimit} collections.");
                }
            }

            var collection = _mapper.Map<Collection>(collectionToCreate);

            _repository.Add(collection);

            await _repository.SaveChangesAsync();

            return CreatedAtRoute("GetCollection", new { collectionId = collection.Id }, collection);
        }

        [HttpPut("{collectionId}")]
        public async Task<IActionResult> Update(int collectionId, CollectionToUpdateDto collectionToUpdate)
        {
            var collection = await _repository.Get(collectionId);

            if (collection.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            collectionToUpdate.Map(collection);

            _repository.Update(collection);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{collectionId}/policy/{policy}")]
        public async Task<IActionResult> ChangeAccessPolicy(int collectionId, string policy)
        {
            var collection = await _repository.Get(collectionId);
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

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{collectionId}", Name = "GetCollection")]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            var collection = await _repository.Get(collectionId);
            if (collection == null)
            {
                return BadRequest();
            }
            var collectionToReturn = _mapper.Map<CollectionToReturnDto>(collection);
            collectionToReturn.CardsCount = await _repository.Find(c => c.Id == collectionId).Select(c => c.Cards).CountAsync();
            collectionToReturn.FollowersCount = await _repository.GetFollowersCount(collection.Id);
            return Ok(collectionToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetCollections([FromQuery] CollectionParams filters)
        {
            filters.IncludePrivateCollections = true;
            if (!User.IsInRole(AuthHelper.AdminRole))
            {
                if (filters.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    filters.IncludePrivateCollections = false;
            }
            var query = _repository.All();

            if (filters.AuthorId.HasValue)
            {
                query = query.Where(c => c.AuthorId == filters.AuthorId);
            }
            if (!string.IsNullOrWhiteSpace(filters.Title))
            {
                query = query.Where(c => c.Title.Contains(filters.Title));
            }

            var collections = await PagedList<Collection>.CreateAsync(query, filters);

            var collectionToReturns = _mapper.Map<IEnumerable<CollectionToReturnDto>>(collections);

            foreach (var collection in collectionToReturns)
            {
                collection.CardsCount = await _repository.GetCollectionsCardsCount(collection.Id);
                collection.FollowersCount = await _repository.GetFollowersCount(collection.Id);
            }
            Response.AddPagination(collections);

            return Ok(collectionToReturns);
        }
    }
}
