﻿using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Tests.CollectionManagementTests
{
    class CollectionControllerCreateTests
    {
        [Test]
        public void ShouldReturnUnauthorizedIfUserIdIsDifferentFromAuthorIdInCollection()
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


            var sut = new CollectionController(repo.Object, mapper.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var collectionToCreateDto = new CollectionCreateDto { Id = collectionId, AuthorId = userId + 1 };
            mapper.Setup(m => m.Map<Collection>(collectionToCreateDto)).Returns(new Collection { Id = collectionId, AuthorId = userId + 1 });

            var result = sut.Create(collectionToCreateDto).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(UnauthorizedResult), result.GetType());
        }

        [Test]
        public void ShouldReturnCreatedAtRoute()
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


            var sut = new CollectionController(repo.Object, mapper.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            repo.Setup(t => t.SaveAll()).Returns(Task.FromResult(true));

            var collectionToCreateDto = new CollectionCreateDto { Id = collectionId, AuthorId = userId };
            mapper.Setup(m => m.Map<Collection>(collectionToCreateDto)).Returns(new Collection { Id = collectionId, AuthorId = userId });

            var result = sut.Create(collectionToCreateDto).GetAwaiter().GetResult();

            Assert.AreEqual(typeof(CreatedAtRouteResult), result.GetType());
        }
    }
}
