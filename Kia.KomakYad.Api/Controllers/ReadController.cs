﻿using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILeitnerRepository _repo;

        public ReadController(IMapper mapper, ILeitnerRepository repos)
        {
            _mapper = mapper;
            _repo = repos;
        }

        [HttpGet("Collection/{readCollectionId}/User/{userId}/Deck/{deck}/Cards")]
        public async Task<IActionResult> GetTodayCards(int readCollectionId, int userId, byte deck, [FromQuery]ReadCardParams readCardParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var readCollection = await _repo.GetReadCollection(readCollectionId);
            if (readCollection == null)
            {
                return NotFound($"There is no data for this ID {readCollectionId}");
            }
            if (readCollection.OwnerId != userId)
            {
                return Unauthorized();
            }

            readCardParams.PageSize = readCollection.ReadPerDay;
            readCardParams.Deck = deck;

            var cards = await _repo.GetReadCards(readCollectionId, readCardParams);

            if (cards == null)
            {
                return NotFound();
            }

            var readCards = _mapper.Map<PagedList<ReadCardToReturnDto>>(cards);

            Response.AddPagination(cards);

            return Ok(readCards);
        }

        [HttpPatch("Card/{readcardId}/User/{userId}/Deck/{deck}/Move")]
        public async Task<IActionResult> MoveCard(int userId, int readcardId, [Range(0, 6)]byte deck)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var readCard = await _repo.GetReadCard(readcardId);

            if (readCard == null)
                return NotFound("Card not found");

            if(readCard.Due > DateTime.Now)
            {
                return BadRequest("You can't move a card it is not dued.");
            }

            readCard.PreviousDeck = readCard.CurrentDeck;
            readCard.CurrentDeck = deck;
            readCard.Due = deck.GetDue();
            readCard.LastChanged = DateTime.Now;

            if (await _repo.SaveAll())
                return NoContent();

            throw new System.Exception($"Unable to move card");
        }
    }
}