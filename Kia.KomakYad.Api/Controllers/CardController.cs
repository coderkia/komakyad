﻿using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AuthHelper.ReadPolicy)]
    public class CardController : ControllerBase
    {
        private ILeitnerRepository _repo;
        private readonly IMapper _mapper;

        public CardController(ILeitnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard(CardCreateDto cardToCreate)
        {
            var collection = await _repo.GetCollection(cardToCreate.CollectionId);
            if (collection?.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (int.TryParse(User.FindFirst(CustomClaimTypes.CardLimit)?.Value, out int cardLimit))
            {
                if(await _repo.GetCardsCount(collection.Id) > cardLimit)
                {
                    return BadRequest($"You cannot add more than {cardLimit} cards to a collection");
                }
            }
            var card = _mapper.Map<Card>(cardToCreate);
            _repo.Add<Card>(card);

            if (await _repo.SaveAll())
            {
                var cardToReturn = _mapper.Map<CardToReturnDto>(card);
                return CreatedAtRoute("GetCard", new { cardId = card.Id }, cardToReturn);
            }
            throw new System.Exception("Card save process failed");
        }

        [HttpGet("{cardId}", Name = "GetCard")]
        public async Task<IActionResult> GetCard(int cardId)
        {
            var card = await _repo.GetCardById(cardId);
            if (card == null)
            {
                return BadRequest();
            }
            var cardToReturnDto = _mapper.Map<CardToReturnDto>(card);
            return Ok(cardToReturnDto);
        }



        [HttpPut]
        public async Task<IActionResult> UpdateCard(CardCreateDto cardToCreate)
        {
            var author = (await _repo.GetCollection(cardToCreate.CollectionId));
            if (author.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (!await _repo.CheckCardExists(cardToCreate.Id))
            {
                return BadRequest();
            }

            var card = _mapper.Map<Card>(cardToCreate);
            _repo.Update<Card>(card);

            if (await _repo.SaveAll())
            {
                var cardToReturn = _mapper.Map<CardToReturnDto>(card);
                return CreatedAtRoute("GetCard", new { cardId = card.Id }, cardToReturn);
            }
            throw new System.Exception("Card save process failed");
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] CardParams cardParams)
        {
            if (!User.IsInRole(AuthHelper.AdminRole))
            {
                var collection = (await _repo.GetCollection(cardParams.CollectionId));
                if (collection.IsPrivate && collection.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();
            }

            var cards = await _repo.GetCards(cardParams);

            var cardToReturnDto = _mapper.Map<IEnumerable<CardToSearchReturnDto>>(cards);

            Response.AddPagination(cards);

            return Ok(cardToReturnDto);
        }
    }
}