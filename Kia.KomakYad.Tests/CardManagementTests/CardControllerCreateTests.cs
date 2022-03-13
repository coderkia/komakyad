using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Kia.KomakYad.Tests.CardManagementTests
{
    public class CardControllerCreateTests
    {

        [Fact]
        public void ShouldReturnUnauthorizedIfUserIdIsDifferentFromUserInCollection()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                new User { Id = 10, EmailConfirmed = true, UserName = "user 10"},
                new User { Id = 2, EmailConfirmed = true, UserName = "user 2" }
            });
            var collectionId = 12;
            var userId = 10;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);


            var sut = new CardController(repo.Object, mapper.Object, userManager.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult(new Collection { AuthorId = userId + 1 }));


            var result = sut.CreateCard(new Api.Dtos.CardCreateDto { CollectionId = collectionId }).GetAwaiter().GetResult();

            Assert.Equal(typeof(UnauthorizedResult), result.GetType());
        }

        [Fact]
        public void ShouldReturnCardDetailsAfterSuccessfullySaved()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                new User { Id = 10, EmailConfirmed = true, UserName = "user 10"},
                new User { Id = 2, EmailConfirmed = true, UserName = "user 2" }
            });
            var collectionId = 12;
            var userId = 10;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);


            var sut = new CardController(repo.Object, mapper.Object, userManager.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var cardDto = new Api.Dtos.CardCreateDto { CollectionId = collectionId };
            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult(new Collection { AuthorId = userId }));
            repo.Setup(t => t.SaveAll()).Returns(Task.FromResult(true));

            mapper.Setup(t => t.Map<Card>(cardDto)).Returns(new Card
            {
                CollectionId = cardDto.CollectionId
            });

            var result = sut.CreateCard(cardDto).GetAwaiter().GetResult();

            Assert.Equal(typeof(CreatedAtRouteResult), result.GetType());
        }
    }
}
