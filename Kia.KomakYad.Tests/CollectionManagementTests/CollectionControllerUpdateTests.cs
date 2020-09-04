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

namespace Kia.KomakYad.Tests.CollectionManagementTests
{
    class CollectionControllerUpdateTests
    {
        [Test]
        public void ShouldReturnUnauthorizedIfUserIdIsDifferentFromAuthorIdInCollection()
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

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);


            var sut = new CollectionController(repo.Object, mapper.Object, userManager.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
            var expectedTitle = "Updated title";
            var expectedDescription = "Updated Description";
            var collectionToUpdateDto = new CollectionToUpdateDto { Title = expectedTitle, Description = expectedDescription };

            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult(new Collection { AuthorId = userId + 1 }));

            var result = sut.Update(collectionId, collectionToUpdateDto).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(UnauthorizedResult), result.GetType());
        }

        [Test]
        public void ShouldReturnCreatedAtRoute()
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

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);


            var sut = new CollectionController(repo.Object, mapper.Object, userManager.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
            var expectedTitle = "Updated title";
            var expectedDescription = "Updated Description";
            var collectionToUpdateDto = new CollectionToUpdateDto { Title = expectedTitle, Description = expectedDescription };
            var inDbCollection = new Collection { AuthorId = userId, Id = collectionId };

            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult(inDbCollection));
            repo.Setup(t => t.SaveAll()).Returns(Task.FromResult(true));

            var result = sut.Update(collectionId, collectionToUpdateDto).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(NoContentResult), result.GetType());
            Assert.AreEqual(expectedTitle, inDbCollection.Title);
            Assert.AreEqual(expectedDescription, inDbCollection.Description);
        }
    }
}
