using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Kia.KomakYad.Tests.CardManagementTests
{
    class CardControllerGetCardTests
    {
        [Test]
        public void ShouldReturnCardWhenCallingGetCard()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var cardId = 12;
            var expectedCard = new Card
            {
                Id = cardId
            };
            var sut = new CardController(repo.Object, mapper.Object);

            repo.Setup(t => t.GetCardById(cardId))
                .Returns(Task.FromResult(expectedCard));

            mapper.Setup(t => t.Map<CardToReturnDto>(expectedCard)).Returns(new CardToReturnDto
            {
                Id = cardId
            });

            var result = sut.GetCard(cardId).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        }

        [Test]
        public void ShouldReturnBadRequestWhenCallingGetCardIfCardIdIsNotExists()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var cardId = 12;
            var expectedCard = new Card
            {
                Id = 12
            };
            var sut = new CardController(repo.Object, mapper.Object);

            repo.Setup(t => t.GetCardById(cardId))
                .Returns(Task.FromResult((Card)null));
            
            var result = sut.GetCard(cardId).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(BadRequestResult), result.GetType());
        }
    }
}
