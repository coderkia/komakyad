using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Kia.KomakYad.Tests.RepoTests
{
    public class CardRepoTests
    {
        private int _dbCount = 0;
        private ILeitnerRepository sut;
        //private DbContextOptions<DataContext> _options;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "CardRepoTests" + ++_dbCount)
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

        [Test]
        public void GetCards_FilterCardsByCollectionId()
        {
            var cardParams = new ReadCardParams
            {
                CollectionId = 1
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.IsTrue(actual.Any(c => c.CollectionId == cardParams.CollectionId));
        }

        [Test]
        public void GetCards_FilterCardsByAnswer()
        {
            var cardParams = new ReadCardParams
            {
                CollectionId = 1,
                Answer = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.IsTrue(actual.Any(c => c.Answer.Contains(cardParams.Answer)));
        }

        [Test]
        public void GetCards_FilterCardsByQuestion()
        {
            var cardParams = new ReadCardParams
            {
                CollectionId = 1,
                Question = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.IsTrue(actual.Any(c => c.Question.Contains(cardParams.Question)));
        }

        [Test]
        public void GetCards_FilterCardsByExample()
        {
            var cardParams = new ReadCardParams
            {
                CollectionId = 1,
                Example = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.IsTrue(actual.Any(c => c.Example.Contains(cardParams.Example)));
        }
        [Test]
        public void GetCards_OrderByIdIsWorking()
        {
            var cardParams = new ReadCardParams
            {
                CollectionId = 1,
                OrderBy = "Id"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.Greater(actual.First().Id, actual.Last().Id);
        }

        [Test]
        public void GetCards_FilterCardsConditionsIsAnd()
        {
            var cardParams = new ReadCardParams
            {
                CollectionId = 2,
                Answer = "3"
            };

            var actual = sut.GetCards(cardParams).GetAwaiter().GetResult();

            Assert.IsFalse(actual.Any());
        }
    }
}
