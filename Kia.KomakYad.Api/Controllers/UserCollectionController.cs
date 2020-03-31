using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Extensions;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/collection/{collectionId}/user/{userId}")]
    [ApiController]
    public class UserCollectionController : ControllerBase
    {
        private readonly ILeitnerRepository _repo;
        private readonly IMapper _mapper;

        public UserCollectionController(ILeitnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserCollection(int collectionId, int ownerId, UserCollectionToCreateDto userCollectionToCreate)
        {
            if (ownerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            if (await _repo.IsUserCollectionExistAsync(collectionId, ownerId))
            {
                return BadRequest("The collection is already in this user's collections");
            }
            var readCollection = new ReadCollection
            {
                CollectionId = collectionId,
                OwnerId = ownerId,
                ReadPerDay = (byte)userCollectionToCreate.ReadPerDay,
            };

            _repo.Add(readCollection);

            if (!await _repo.SaveAll())
            {
                throw new Exception("Unable to save data.");
            }

            var cards = await _repo.GetCards(collectionId);
            var readCards = _mapper.Map<IEnumerable<ReadCard>>(cards);

            readCards.Map(c =>
            {
                c.OwnerId = ownerId;
                c.ReadCollectionId = readCollection.Id;
            }).ToList();

            _repo.Add(readCards);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("Unable to transfer cards.");
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

        [HttpGet("Cards")]
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