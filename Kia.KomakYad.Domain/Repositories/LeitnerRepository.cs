using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.DataAccess;
using Kia.KomakYad.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Kia.KomakYad.Common.Helpers;

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

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<PagedList<Collection>> GetCollectionsAsync(CollectionParams collectionParams)
        {
            var collections = _context.UserCollections.Where(c => c.UserId == collectionParams.UserId)
                .Include(m => m.Collection).Select(c => c.Collection);

            return await PagedList<Collection>.CreateAsync(collections, collectionParams.PageNumber, collectionParams.PageSize);
        }

        public IQueryable<DueCard> GetDueCards(int collectionId, int userId, byte deck = byte.MaxValue)
        {
            var query = _context.DueCards.Where(c => c.OwnerId == userId);

            if (!deck.AllDeck())
                query.Where(c => c.CurrentDeck == deck);

            return query.Include(c => c.Card).Where(c => c.Card.CollectionId == collectionId);
        }

        public async Task<int> GetDueCardCount(int collectionId, int userId, byte deck = byte.MaxValue)
        {
            return await GetDueCards(collectionId, userId, deck).CountAsync();
        }

        public IQueryable<DueCard> GetReadCards(int collectionId, int userId, byte deck = byte.MaxValue)
        {
            var query = GetDueCards(collectionId, userId, deck).Where(c => c.LastChanged > DateTime.Now.Date);

            if (!deck.AllDeck())
            {
                query.Where(c => c.CurrentDeck == deck);
            }

            return query;
        }

        public async Task<int> GetFailedCount(int collectionId, int userId, byte deck = byte.MaxValue)
        {
            return await GetReadCards(collectionId, userId, deck).CountAsync(c => c.PreviouseDeck == c.CurrentDeck + 1);
        }

        public async Task<int> GeSucceedCount(int collectionId, int userId, byte deck = byte.MaxValue)
        {
            return await GetReadCards(collectionId, userId, deck).CountAsync(c => c.PreviouseDeck == c.CurrentDeck - 1);
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

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

    }
}
