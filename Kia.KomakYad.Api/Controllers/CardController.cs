﻿using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var author = (await _repo.GetCollection(cardToCreate.CollectionId));
            if (author.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
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
        [AllowAnonymous]
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

            var card = await _repo.GetCardById(cardToCreate.Id);

            _repo.Update<Card>(card);

            if (await _repo.SaveAll())
            {
                var cardToReturn = _mapper.Map<CardToReturnDto>(card);
                return CreatedAtRoute("GetCard", new { cardId = card.Id }, cardToReturn);
            }
            throw new System.Exception("Card save process failed");
        }
    }
}