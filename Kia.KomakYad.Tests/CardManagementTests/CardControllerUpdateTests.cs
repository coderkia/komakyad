using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Kia.KomakYad.Tests.CardManagementTests
{
    class CardControllerUpdateTests
    {
        [Test]
        public void ShouldReturnUnauthorizedIfUserIdIsDifferentFromUserInCollection()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                 new User { Id = 10, UserName = "user 10"},
                 new User { Id = 2, UserName = "user 2" }
            });
            var collectionId = 12;
            var userId = 10;
            var cardId = 55;
            var expectedCard = new Card
            {
                Id = cardId
            };

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            repo.Setup(t => t.GetCardById(cardId)).Returns(Task.FromResult(expectedCard));

            var sut = new CardController(repo.Object, mapper.Object, userManager.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            //Return unauthorized User ID
            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult(new Collection { AuthorId = userId + 1 }));


            var result = sut.UpdateCard(new Api.Dtos.CardCreateDto { CollectionId = collectionId }).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(UnauthorizedResult), result.GetType());
        }


        [Test]
        public void ShouldReturnCardDetailsAfterSuccessfullySaved()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                 new User { Id = 10, UserName = "user 10"},
                 new User { Id = 2, UserName = "user 2" }
            });
            var collectionId = 12;
            var userId = 10;
            var cardId = 55;
            var expectedCard = new Card
            {
                Id = cardId
            };

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            repo.Setup(t => t.GetCardById(cardId)).Returns(Task.FromResult(expectedCard));
            repo.Setup(t => t.CheckCardExists(cardId)).Returns(Task.FromResult(true));

            var sut = new CardController(repo.Object, mapper.Object, userManager.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var cardDto = new CardCreateDto { CollectionId = collectionId, Id = cardId };

            //Return authorized User ID
            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult(new Collection { AuthorId = userId }));
            repo.Setup(t => t.SaveAll()).Returns(Task.FromResult(true));

            mapper.Setup(t => t.Map<Card>(cardDto)).Returns(new Card
            {
                CollectionId = cardDto.CollectionId
            });

            var result = sut.UpdateCard(cardDto).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(CreatedAtRouteResult), result.GetType());
        }

        [Test]
        public void ShouldReturnBadRequestIfCardIsNotFound()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                 new User { Id = 10, UserName = "user 10"},
                 new User { Id = 2, UserName = "user 2" }
            });
            var collectionId = 12;
            var userId = 10;
            var cardId = 55;
            var expectedCard = new Card
            {
                Id = cardId
            };

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            //Card is not found
            repo.Setup(t => t.GetCardById(cardId)).Returns(Task.FromResult((Card)null));

            var sut = new CardController(repo.Object, mapper.Object, userManager.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var cardDto = new Api.Dtos.CardCreateDto { CollectionId = collectionId };

            //Return authorized User ID
            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult(new Collection { AuthorId = userId }));
            repo.Setup(t => t.SaveAll()).Returns(Task.FromResult(true));

            mapper.Setup(t => t.Map<Card>(cardDto)).Returns(new Card
            {
                CollectionId = cardDto.CollectionId
            });

            var result = sut.UpdateCard(cardDto).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(BadRequestResult), result.GetType());
        }
    }
}
