using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Kia.KomakYad.Tests.RepoTests
{
    public class CardRepoTests
    {
        private ILeitnerRepository sut;
        //private DbContextOptions<DataContext> _options;

        public CardRepoTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new DataContext(options);

            var cards = new List<Card>
            {
                new Card
                {
                    Id=1,
                    CollectionId = 1,
                    Answer ="A1",
                    Question="Q1",
                    Example="E1"
                },
                new Card
                {
                    Id=2,
                    CollectionId = 1,
                    Answer ="A2",
                    Question="Q2",
                    Example="E2"
                },
                new Card
                {
                    Id=3,
                    CollectionId = 1,
                    Answer ="A3",
                    Question="Q3",
                    Example="E3"
                },
                new Card
                {
                    Id=4,
                    CollectionId = 2,
                    Answer ="A4",
                    Question="Q4",
                    Example="E4"
                },
                new Card
                {
                    Id=5,
                    CollectionId = 2,
                    Answer ="A5",
                    Question="Q5",
                    Example="E5"
                }
            };

            dbContext.Cards.AddRange(cards);
            dbContext.SaveChanges();

            sut = new LeitnerRepository(dbContext);
        }

        [Fact]
        public void GetCards_FilterCardsByCollectionId()
        {
            var cardParams = new CardParams
            {
                CollectionId = 1
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.Contains(actual, c => c.CollectionId == cardParams.CollectionId);
        }

        [Fact]
        public void GetCards_FilterCardsByAnswer()
        {
            var cardParams = new CardParams
            {
                CollectionId = 1,
                Answer = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.Contains(actual, c => c.Answer.Contains(cardParams.Answer));
        }

        [Fact]
        public void GetCards_FilterCardsByQuestion()
        {
            var cardParams = new CardParams
            {
                CollectionId = 1,
                Question = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.Contains(actual, c => c.Question.Contains(cardParams.Question));
        }

        [Fact]
        public void GetCards_FilterCardsByExample()
        {
            var cardParams = new CardParams
            {
                CollectionId = 1,
                Example = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.Contains(actual, c => c.Example.Contains(cardParams.Example));
        }
        [Fact]
        public void GetCards_OrderByIdIsWorking()
        {
            var cardParams = new CardParams
            {
                CollectionId = 1,
                OrderBy = "Id"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.True(actual.First().Id < actual.Last().Id);
        }

        [Fact]
        public void GetCards_FilterCardsConditionsIsAnd()
        {
            var cardParams = new CardParams
            {
                CollectionId = 2,
                Answer = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.False(actual.Any());
        }
    }
}
