using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kia.KomakYad.Tests.CollectionManagementTests
{
    class CollectionControllerGetCollectionTests
    {
        [Test]
        public void ShouldReturnCollectionWhenCallingGetCollection()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                 new User { Id = 10, UserName = "user 10"},
                 new User { Id = 2, UserName = "user 2" }
            });
            var collectionId = 12;
            var expectedCollection = new Collection
            {
                Id = collectionId
            };
            var sut = new CollectionController(repo.Object, mapper.Object, userManager.Object);

            repo.Setup(t => t.GetCollection(collectionId))
                .Returns(Task.FromResult(expectedCollection));

            mapper.Setup(t => t.Map<CollectionToReturnDto>(expectedCollection)).Returns(new CollectionToReturnDto
            {
                Id = collectionId
            });

            var result = sut.GetCollection(collectionId).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        }

        [Test]
        public void ShouldReturnBadRequestWhenCallingGetCollectionIfCollectionIdIsNotExists()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                 new User { Id = 10, UserName = "user 10"},
                 new User { Id = 2, UserName = "user 2" }
            });
            var collectionId = 12;
            var expectedCollection = new Collection
            {
                Id = collectionId
            };
            var sut = new CollectionController(repo.Object, mapper.Object, userManager.Object);

            repo.Setup(t => t.GetCollection(collectionId)).Returns(Task.FromResult((Collection)null));

            mapper.Setup(t => t.Map<CollectionToReturnDto>(expectedCollection)).Returns(new CollectionToReturnDto
            {
                Id = collectionId
            });

            var result = sut.GetCollection(collectionId).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(BadRequestResult), result.GetType());
        }
    }
}
