using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Common.Helpers;
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
    public class ReadController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILeitnerRepository repos;

        public ReadController(IMapper mapper, ILeitnerRepository repos)
        {
            this.mapper = mapper;
            this.repos = repos;
        }

        [HttpGet("Collection/{readCollectionId}/User/{userId}/Cards")]
        public async Task<IActionResult> GetTodayCards(int readCollectionId, int userId, [FromQuery]ReadCardParams readCardParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var readCollection = await repos.GetReadCollection(readCollectionId);
           if (readCollection == null)
            {
                return NotFound($"There is no data for this ID {readCollectionId}");
            }
            if (readCollection.OwnerId != userId)
            {
                return Unauthorized();
            }

            readCardParams.PageSize = readCollection.ReadPerDay;
            var cards = await repos.GetCardsToRead(readCollectionId, readCardParams);

            if (cards == null)
            {
                return NotFound();
            }

            var readCards = mapper.Map<PagedList<ReadCardToReturnDto>>(cards);
            return Ok(readCards);
        }
    }
}