using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Kia.KomakYad.Common.Helpers;
using System.Collections.Generic;

namespace Kia.KomakYad.Domain.Repositories
{
    public class LeitnerRepository : ILeitnerRepository
    {
        private DataContext _context;
        public LeitnerRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.AddRange(entities);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<PagedList<Collection>> GetUserCollections(int ownerId, CollectionParams filters)
        {
            var collections = _context.ReadCollections.Where(c => c.OwnerId == ownerId)
                .Include(m => m.Collection).Select(c => c.Collection);

            return await PagedList<Collection>.CreateAsync(collections, filters);
        }

        public async Task<PagedList<Collection>> GetCollections(CollectionParams filters)
        {
            var collections = _context.Collections.Include(c => c.Author).AsQueryable();

            if (filters.AuthorId.HasValue)
                collections = collections.Where(c => c.AuthorId == filters.AuthorId);

            if (!string.IsNullOrWhiteSpace(filters.Title))
                collections = collections.Where(c => c.Title.Contains(filters.Title));

            return await PagedList<Collection>.CreateAsync(collections, filters);
        }

        public async Task<int> GetDueCardCount(int readCollectionId, ReadCardParams filters)
        {
            filters.OnlyDued = true;
            var query = GetReadCardsQuery(readCollectionId, filters);
            return await query.CountAsync();
        }

        public async Task<int> GetFailedCount(int readCollectionId, ReadCardParams filters)
        {
            filters.OnlyDued = false;
            var query = GetReadCardsQuery(readCollectionId, filters);
            query = query.Where(c => c.LastChanged > DateTime.Now.Date);
            return await query.CountAsync(c => c.PreviousDeck == c.CurrentDeck + 1);
        }

        public async Task<int> GeSucceedCount(int readCollectionId, ReadCardParams filters)
        {
            filters.OnlyDued = false;
            var query = GetReadCardsQuery(readCollectionId, filters);
            query = query.Where(c => c.LastChanged > DateTime.Now.Date);
            return await query.CountAsync(c => c.PreviousDeck == c.CurrentDeck - 1);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Collection> GetCollection(int collectionId)
        {
            return await _context.Collections.FirstOrDefaultAsync(c => c.Id == collectionId);
        }
        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams filters)
        {
            var users = _context.Users.AsQueryable();

            users = users.Where(u => u.Id != filters.UserId);

            return await PagedList<User>.CreateAsync(users, filters);
        }

        public async Task<Card> GetCardById(int cardId)
        {
            return await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardId);
        }

        public async Task<bool> IsUserCollectionExistAsync(int collectionId, int ownerId)
        {
            return await _context.ReadCollections.AnyAsync(c => c.CollectionId == collectionId && c.OwnerId == ownerId);
        }

        public async Task<PagedList<Card>> GetCards(CardParams filters)
        {
            var cards = _context.Cards.AsQueryable();

            cards = cards.Where(c => c.CollectionId == filters.CollectionId);
            if (!string.IsNullOrWhiteSpace(filters.Answer))
            {
                cards = cards.Where(c => c.Answer.Contains(filters.Answer));
            }
            if (!string.IsNullOrWhiteSpace(filters.Question))
            {
                cards = cards.Where(c => c.Question.Contains(filters.Question));
            }
            if (!string.IsNullOrWhiteSpace(filters.Example))
            {
                cards = cards.Where(c => c.Example.Contains(filters.Example));
            }
            switch (filters.OrderBy?.ToLower())
            {
                case "id":
                    cards = cards.OrderByDescending(c => c.Id);
                    break;
            }
            return await PagedList<Card>.CreateAsync(cards, filters);
        }

        private IQueryable<ReadCard> GetReadCardsQuery(int readCollectionId, ReadCardParams filters)
        {
            var query = _context.ReadCards.Where(c => c.ReadCollectionId == readCollectionId).AsQueryable();
            query = query.Include(c => c.Card);
            if (filters.Deck.HasValue)
            {
                query = query.Where(c => c.CurrentDeck == filters.Deck);
            }
            if (!string.IsNullOrWhiteSpace(filters.Answer))
            {
                query = query.Where(c => c.Card.Answer.Contains(filters.Answer));
            }
            if (!string.IsNullOrWhiteSpace(filters.Question))
            {
                query = query.Where(c => c.Card.Question.Contains(filters.Question));
            }
            if (!string.IsNullOrWhiteSpace(filters.Example))
            {
                query = query.Where(c => c.Card.Example.Contains(filters.Example));
            }
            if (filters.OnlyDued)
            {
                query = query.Where(c => c.Due < DateTime.Now);
            }

            return query;
        }

        public async Task<PagedList<ReadCard>> GetReadCards(int readCollectionId, ReadCardParams filters)
        {
            var query = GetReadCardsQuery(readCollectionId, filters);
            return await PagedList<ReadCard>.CreateAsync(query, filters);
        }

        public async Task<IEnumerable<Card>> GetCards(int collectionId)
        {
            return await _context.Cards.Where(c => c.CollectionId == collectionId).ToListAsync();
        }

        public async Task<ReadCollection> GetReadCollection(int readCollectionId)
        {
            return await _context.ReadCollections.FirstOrDefaultAsync(c => c.Id == readCollectionId);
        }

        public async Task<ReadCard> GetReadCard(int readcardId)
        {
            return await _context.ReadCards.FirstOrDefaultAsync(c => c.Id == readcardId);
        }

        public async Task<PagedList<ReadCollection>> GetReadCollections(ReadCollectionParams filters)
        {
            var query = _context.ReadCollections.AsQueryable();
            if (!filters.IncludingDeleted)
            {
                query = query.Where(c => c.Deleted != true);
            }
            if (filters.OwnerId.HasValue)
            {
                query = query.Where(c => c.OwnerId == filters.OwnerId);
            }

            return await PagedList<ReadCollection>.CreateAsync(query, filters);
        }
    }
}
