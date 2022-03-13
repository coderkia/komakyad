using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Tests.CollectionManagementTests
{
    public class CollectionControllerUpdateTests
    {
        [Fact]
        public void ShouldReturnUnauthorizedIfUserIdIsDifferentFromAuthorIdInCollection()
        {
            var repo = new Mock<ICollectionRespository>();
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

            repo.Setup(t => t.Get(collectionId)).Returns(Task.FromResult(new Collection { AuthorId = userId + 1 }));

            var result = sut.Update(collectionId, collectionToUpdateDto).GetAwaiter().GetResult();

            Assert.Equal(typeof(UnauthorizedResult), result.GetType());
        }

        [Fact]
        public void ShouldReturnCreatedAtRoute()
        {
            var repo = new Mock<ICollectionRespository>();
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

            repo.Setup(t => t.Get(collectionId)).Returns(Task.FromResult(inDbCollection));

            var result = sut.Update(collectionId, collectionToUpdateDto).GetAwaiter().GetResult();

            Assert.Equal(typeof(NoContentResult), result.GetType());
            Assert.Equal(expectedTitle, inDbCollection.Title);
            Assert.Equal(expectedDescription, inDbCollection.Description);
        }
    }
}
