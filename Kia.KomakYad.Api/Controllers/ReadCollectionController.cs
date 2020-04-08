using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Extensions;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadCollectionController : ControllerBase
    {
        private readonly ILeitnerRepository _repo;
        private readonly IMapper _mapper;

        public ReadCollectionController(ILeitnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("Collection({collectionId})/User({userId})")]
        public async Task<IActionResult> AddUserCollection(int collectionId, int userId, UserCollectionToCreateDto userCollectionToCreate)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            if (await _repo.IsUserCollectionExistAsync(collectionId, userId))
            {
                return BadRequest("The collection is already in this user's collections");
            }
            var readCollection = new ReadCollection
            {
                CollectionId = collectionId,
                OwnerId = userId,
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
                c.Id = 0;
                c.OwnerId = userId;
                c.ReadCollectionId = readCollection.Id;
            }).ToList();

            _repo.AddRange(readCards);

            if (await _repo.SaveAll())
            {
                return Ok(readCollection);
            }
            throw new Exception("Unable to transfer cards.");
        }

        [HttpGet("{readCollectionId}/User({userId})/overview")]
        public async Task<IActionResult> GetTodayCardOverview(int readCollectionId, int userId, [FromQuery]byte? deck)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            if (deck > 6)
            {
                return BadRequest("the deck is not valid");
            }
            var overview = new TodayOverview
            {
                Deck = deck ,
                CollectionId = readCollectionId,
                OwnerId = userId
            };
            var filters = new ReadCardParams
            {
                Deck = deck
            };
            overview.DueCount = await _repo.GetDueCardCount(readCollectionId, filters);
            overview.DownCount = await _repo.GetFailedCount(readCollectionId, filters);
            overview.UpCount = await _repo.GeSucceedCount(readCollectionId, filters);

            return Ok(overview);
        }

        [HttpGet("Cards")]
        public async Task<IActionResult> GetCards(int readCollectionId, [FromQuery]CardParams cardParams)
        {
            cardParams.CollectionId = readCollectionId;
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