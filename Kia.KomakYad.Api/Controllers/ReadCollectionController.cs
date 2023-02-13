using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Api.Models;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AuthHelper.ReadPolicy)]
    public class ReadCollectionController : ControllerBase
    {
        private readonly ILeitnerRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ReadCollectionController(ILeitnerRepository repo, IMapper mapper, UserManager<User> userManager)
        {
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("Collection({collectionId})/User({userId})")]
        public async Task<IActionResult> AddUserCollection(int collectionId, int userId, UserCollectionToCreateDto userCollectionToCreate)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (!user.EmailConfirmed)
            {
                return StatusCode(403, "Confirm your eamil first");
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
                LastUpdateCheck = DateTime.Now
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
            });

            _repo.AddRange(readCards);

            if (await _repo.SaveAll())
            {
                return Ok(readCollection);
            }
            throw new Exception("Unable to transfer cards.");
        }

        [HttpGet("{readCollectionId}/User/{userId}/overview")]
        [HttpGet("{readCollectionId}/User/{userId}/deck/{deck}/overview")]
        public async Task<IActionResult> GetTodayCardOverview(int readCollectionId, int userId, byte? deck)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            if (deck > 6)
            {
                return BadRequest("The deck is not valid");
            }
            var overview = new TodayOverview
            {
                Deck = deck,
                ReadCollectionId = readCollectionId,
                OwnerId = userId,
            };
            var filters = new ReadCardParams
            {
                Deck = deck
            };
            overview.DueCount = await _repo.GetDueCardCount(readCollectionId, filters);
            overview.TotalCount = await _repo.GetTotalCount(readCollectionId, filters);
            overview.DownCount = await _repo.GetFailedCount(readCollectionId, filters);
            overview.UpCount = await _repo.GetSucceedCount(readCollectionId, filters);

            return Ok(overview);
        }

        public async Task<IActionResult> CheckForUpdates(int readCollectionId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var readCollection = await _repo.GetReadCollection(readCollectionId);
            var newCards = _repo.GetNewCardsToRead(readCollection.Id, readCollection.CollectionId);

            if (newCards == null)
            {
                return NoContent();
            }

            var readCards = _mapper.Map<IEnumerable<ReadCard>>(newCards);

            readCards.Map(c =>
            {
                c.Id = 0;
                c.OwnerId = userId;
                c.ReadCollectionId = readCollection.Id;
            }).ToList();

            _repo.AddRange(readCards);

            if (await _repo.SaveAll())
            {
                var cardsToReturnDto = _mapper.Map<List<CardToReturnDto>>(newCards);
                return Ok(cardsToReturnDto);
            }
            throw new Exception("Unable to transfer cards.");

        }

        [HttpGet("Cards")]
        public async Task<IActionResult> GetCards(int readCollectionId, [FromQuery] CardParams cardParams)
        {
            cardParams.CollectionId = readCollectionId;
            var readCollection = await _repo.GetReadCollection(readCollectionId);
            if (readCollection == null)
            {
                return NotFound("The id not found.");
            }
            cardParams.PageSize = readCollection.ReadPerDay;
            var cards = await _repo.GetCards(cardParams);
            if (cards == null)
            {
                return BadRequest();
            }
            var cardToReturnDtos = _mapper.Map<IEnumerable<CardToReturnDto>>(cards);

            Response.AddPagination(cards.CurrentPage, cards.PageSize, cards.TotalCount, cards.TotalPages);

            return Ok(cardToReturnDtos);
        }

        [HttpPatch("Remove({readCollectionId})")]
        public async Task<IActionResult> Remove(int readCollectionId)
        {
            var readCollection = await _repo.GetReadCollection(readCollectionId);
            if (readCollection.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            readCollection.Deleted = true;

            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("Unable to remove read collection.");
        }

        [HttpPatch("Restore({readCollectionId})")]
        public async Task<IActionResult> Restore(int readCollectionId)
        {
            var readCollection = await _repo.GetReadCollection(readCollectionId);
            if (readCollection.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            readCollection.Deleted = false;

            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("Unable to restore read collection.");
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll([FromQuery] ReadCollectionParams readCollectionParams)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            readCollectionParams.OwnerId = ownerId;
            var readCollections = await _repo.GetReadCollections(readCollectionParams);

            var readCollectionsToReturn = _mapper.Map<IEnumerable<ReadCollectionToReturnDto>>(readCollections);
            Response.AddPagination(readCollections.CurrentPage, readCollections.PageSize, readCollections.TotalCount, readCollections.TotalPages);

            return Ok(readCollectionsToReturn);
        }

        [HttpPost("{readCollectionId}/MoveCards")]
        public async Task<IActionResult> MoveCards(int readCollectionId, MoveCardModel model)
        {
            var readCollection = await _repo.GetReadCollection(readCollectionId);
            if (readCollection == null)
            {
                return NotFound($"There is no ReadCollection with ID {readCollectionId}");
            }
            if (readCollection.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var filters = new ReadCardParams
            {
                Deck = (byte?)model.TargetDeck,
                OnlyDued = false,
            };
            filters.BypassMaxPageSize(int.MaxValue);
            var cards = await _repo.GetReadCards(readCollectionId, filters);
            if (!cards.Any())
            {
                return Ok(new { Message = $"No cards are in deck { model.TargetDeck?.ToString() ?? "0-6" }" });
            }
            foreach (var card in cards)
            {
                card.CurrentDeck = (byte)model.DestinationDeck;
                card.Due = DateTime.Now.AddDays(-1);
                card.PreviousDeck = 0;
            }
            if (await _repo.SaveAll())
            {
                return Ok(new { Message = $"{ cards.Count() } cards in deck { model.TargetDeck?.ToString() ?? "0-6" } are moved to deck { model.DestinationDeck }" });
            }
            throw new Exception("Unable to move cards.");
        }
    }
}