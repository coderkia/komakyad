using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace Kia.KomakYad.Tests.CardManagementTests
{
    class CardControllerCreateTests
    {

        [Test]
        public void ShouldReturnUnauthorizedIfUserIdIsDifferentFromUserInCollection()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var collectionId = 12;
            var userId = 10;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);


            var sut = new CardController(repo.Object, mapper.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            repo.Setup(t => t.GetCollection(collectionId))
                .Returns(Task.FromResult(new Collection { AuthorId = userId + 1 }));


            var result = sut.CreateCard(new Api.Dtos.CardCreateDto { CollectionId = collectionId }).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(UnauthorizedResult), result.GetType());
        }

        [Test]
        public void ShouldReturnCardDetailsAfterSuccessfullySaved()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var collectionId = 12;
            var userId = 10;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);


            var sut = new CardController(repo.Object, mapper.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var cardDto = new Api.Dtos.CardCreateDto { CollectionId = collectionId };
            repo.Setup(t => t.GetCollection(collectionId))
                .Returns(Task.FromResult(new Collection { AuthorId = userId }));
            repo.Setup(t => t.SaveAll())
                .Returns(Task.FromResult(true));

            mapper.Setup(t => t.Map<Card>(cardDto)).Returns(new Card
            {
                CollectionId = cardDto.CollectionId
            });

            var result = sut.CreateCard(cardDto).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(CreatedAtRouteResult), result.GetType());
        }


    }
}
