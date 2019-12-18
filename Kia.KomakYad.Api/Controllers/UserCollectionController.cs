using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Extensions;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/collection/{collectionId}/user/{userId}")]
    [ApiController]
    public class UserCollectionController : ControllerBase
    {
        private readonly ILeitnerRepository _repo;

        public UserCollectionController(ILeitnerRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserCollection(int collectionId, int userId, UserCollectionToCreateDto userCollectionToCreate)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            if(await _repo.IsUserCollectionExistAsync(collectionId, userId))
            {
                return BadRequest("The collection is already in this user's collections");
            }

            _repo.Add(new UserCollection
            {
                CollectionId = collectionId,
                UserId = userId,
                ReadPerDay = (byte)userCollectionToCreate.ReadPerDay,
            });

            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("Unable to save data.");
        }

        [HttpGet("overview/{deck:int?}")]
        public async Task<IActionResult> GetTodayCardOverview(int collectionId, int userId, byte deck = byte.MaxValue)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            if (!deck.IsAllDeckNeeded() && deck > 6)
            {
                return BadRequest("the deck is not valid");
            }
            var overview = new TodayOverview
            {
                Deck = deck,
                CollectionId = collectionId,
                OwnerId = userId
            };

            overview.DueCount = await _repo.GetDueCardCount(collectionId, userId, deck);
            overview.DownCount = await _repo.GetFailedCount(collectionId, userId, deck);
            overview.UpCount = await _repo.GeSucceedCount(collectionId, userId, deck);

            return Ok(overview);
        }
    }
}