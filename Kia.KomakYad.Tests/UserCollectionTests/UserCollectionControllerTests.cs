using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Tests.UserCollectionTests
{
    public class UserCollectionControllerTests
    {
        [Test]
        public void ShouldReturnUnauthorizedIfOwnerIdIsDifferentFromUserId()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var collectionId = 12;
            var ownerId = 10;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, ownerId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);


            var sut = new ReadCollectionController(repo.Object, mapper.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var userCollectionToCreate = new UserCollectionToCreateDto();


            var result = sut.AddUserCollection(collectionId, ownerId + 1, userCollectionToCreate).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(UnauthorizedResult), result.GetType());
        }

        [Test]
        public void ShouldReturnBadRequestIfItAlreadyExists()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var collectionId = 12;
            var ownerId = 10;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, ownerId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var sut = new ReadCollectionController(repo.Object, mapper.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            repo.Setup(t => t.IsUserCollectionExistAsync(collectionId, ownerId)).Returns(Task.FromResult(true));

            var userCollectionToCreate = new UserCollectionToCreateDto();


            var result = sut.AddUserCollection(collectionId, ownerId, userCollectionToCreate).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
        }


        [Test]
        public void ShouldAddOwnerIdToAllReadCardsEntities()
        {
            var repo = new Mock<ILeitnerRepository>();
            var mapper = new Mock<IMapper>();
            var collectionId = 12;
            var ownerId = 10;
            var readCollectionId = 101;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.NameIdentifier, ownerId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var sut = new ReadCollectionController(repo.Object, mapper.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var readCollection = new ReadCollection
            {
                CollectionId = collectionId,
                OwnerId = ownerId
            };
            var cards = new List<Card>
            {
                new Card{ CollectionId = collectionId, Id = 1},
                new Card{ CollectionId = collectionId, Id = 2},
                new Card{ CollectionId = collectionId, Id = 3},
                new Card{ CollectionId = collectionId, Id = 4},
            };
            var readCards = new List<ReadCard>
            {
                new ReadCard{ CardId =  1},
                new ReadCard{ CardId =  2},
                new ReadCard{ CardId =  3},
                new ReadCard{ CardId =  4},
            };
            mapper.Setup(t => t.Map<IEnumerable<ReadCard>>(cards)).Returns(readCards);

            repo.Setup(t => t.IsUserCollectionExistAsync(collectionId, ownerId)).Returns(Task.FromResult(false));
            repo.Setup(t => t.GetCards(collectionId)).Returns(Task.FromResult(cards.AsEnumerable()));
            repo.Setup(t => t.Add(It.IsAny<ReadCollection>())).Callback<ReadCollection>(c =>
            {
                c.Id = readCollectionId;
            });
            repo.Setup(t => t.Add(readCards));
            repo.Setup(t => t.SaveAll()).Returns(Task.FromResult(true));

            var userCollectionToCreate = new UserCollectionToCreateDto();


            var result = sut.AddUserCollection(collectionId, ownerId, userCollectionToCreate).GetAwaiter().GetResult();

            Assert.AreEqual(readCards.Count, readCards.Count(c => c.OwnerId == ownerId && c.ReadCollectionId == readCollectionId));
        }
    }
}
