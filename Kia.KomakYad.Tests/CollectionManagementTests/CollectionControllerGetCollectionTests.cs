﻿using AutoMapper;
using Kia.KomakYad.Api.Controllers;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Kia.KomakYad.Tests.CollectionManagementTests
{
    public class CollectionControllerGetCollectionTests
    {
        [Fact]
        public void ShouldReturnCollectionWhenCallingGetCollection()
        {
            var repo = new Mock<ICollectionRespository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                new User { Id = 10, EmailConfirmed = true, UserName = "user 10"},
                new User { Id = 2, EmailConfirmed = true, UserName = "user 2" }
            });

            var collectionId = 12;
            var expectedCollection = new Collection
            {
                Id = collectionId
            };
            var sut = new CollectionController(repo.Object, mapper.Object, userManager.Object);

            repo.Setup(t => t.Get(collectionId))
                .Returns(Task.FromResult(expectedCollection));

            repo.Setup(t => t.GetCardsCount(It.IsAny<int>())).Returns(Task.FromResult(10));

            repo.Setup(t => t.GetFollowersCount(It.IsAny<int>())).Returns(Task.FromResult(1));

            mapper.Setup(t => t.Map<CollectionToReturnDto>(expectedCollection)).Returns(new CollectionToReturnDto
            {
                Id = collectionId
            });

            var result = sut.GetCollection(collectionId).GetAwaiter().GetResult();

            Assert.Equal(typeof(OkObjectResult), result.GetType());
        }

        [Fact]
        public void ShouldReturnBadRequestWhenCallingGetCollectionIfCollectionIdIsNotExists()
        {
            var repo = new Mock<ICollectionRespository>();
            var mapper = new Mock<IMapper>();
            var userManager = UserManagerMock.MockUserManager<User>(new List<User>
            {
                new User { Id = 10, EmailConfirmed = true, UserName = "user 10"},
                new User { Id = 2, EmailConfirmed = true, UserName = "user 2" }
            });
            var collectionId = 12;
            var expectedCollection = new Collection
            {
                Id = collectionId
            };
            var sut = new CollectionController(repo.Object, mapper.Object, userManager.Object);

            repo.Setup(t => t.Get(collectionId)).Returns(Task.FromResult((Collection)null));

            mapper.Setup(t => t.Map<CollectionToReturnDto>(expectedCollection)).Returns(new CollectionToReturnDto
            {
                Id = collectionId
            });

            var result = sut.GetCollection(collectionId).GetAwaiter().GetResult();

            Assert.Equal(typeof(BadRequestResult), result.GetType());
        }
    }
}
