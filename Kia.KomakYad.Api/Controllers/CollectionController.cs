﻿using AutoMapper;
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

            throw new System.Exception("Updating the collection failed on save");
        }

        [HttpGet("{collectionId}", Name = "GetCollection")]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            var collections = await _repo.GetCollection(collectionId);
            if (collections == null)
            {
                return BadRequest();
            }
            return Ok(collections);
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

            Response.AddPagination(collections);

            return Ok(collections);
        }
    }
}
