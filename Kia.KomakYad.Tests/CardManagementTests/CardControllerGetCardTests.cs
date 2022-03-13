using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Kia.KomakYad.Tests.CardManagementTests
{
    public class CardControllerGetCardTests
    {
        [Fact]
        public void ShouldReturnCardWhenCallingGetCard()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                new User { Id = 10, EmailConfirmed = true, UserName = "user 10"},
                new User { Id = 2, EmailConfirmed = true, UserName = "user 2" }
            });
            var cardId = 12;
            var expectedCard = new Card
            {
                Id = cardId
            };
            var sut = new CardController(repo.Object, mapper.Object, userManager.Object);

            repo.Setup(t => t.GetCardById(cardId))
                .Returns(Task.FromResult(expectedCard));

            mapper.Setup(t => t.Map<CardToReturnDto>(expectedCard)).Returns(new CardToReturnDto
            {
                Id = cardId
            });

            var result = sut.GetCard(cardId).GetAwaiter().GetResult();

            Assert.Equal(typeof(OkObjectResult), result.GetType());
        }

        [Fact]
        public void ShouldReturnBadRequestWhenCallingGetCardIfCardIdIsNotExists()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                new User { Id = 10, EmailConfirmed = true, UserName = "user 10"},
                new User { Id = 2, EmailConfirmed = true, UserName = "user 2" }
            });
            var cardId = 12;
            var expectedCard = new Card
            {
                Id = 12
            };
            var sut = new CardController(repo.Object, mapper.Object, userManager.Object);

            repo.Setup(t => t.GetCardById(cardId)).Returns(Task.FromResult((Card)null));
            
            var result = sut.GetCard(cardId).GetAwaiter().GetResult();

            Assert.Equal(typeof(BadRequestResult), result.GetType());
        }
    }
}
