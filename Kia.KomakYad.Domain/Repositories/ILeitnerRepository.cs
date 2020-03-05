using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface ILeitnerRepository
    {
        void Update<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<PagedList<Collection>> GetCollections(CollectionParams filters);
        Task<PagedList<Collection>> GetUserCollections(int ownerId, CollectionParams filters);
        IQueryable<ReadCard> GetDueCards(int collectionId, int ownerId, byte deck = byte.MaxValue);
        Task<int> GetDueCardCount(int collectionId, int ownerId, byte deck = byte.MaxValue);
        Task<int> GetFailedCount(int collectionId, int ownerId, byte deck = byte.MaxValue);
        Task<int> GeSucceedCount(int collectionId, int ownerId, byte deck = byte.MaxValue);
        IQueryable<ReadCard> GetReadCards(int collectionId, int ownerId, byte deck = byte.MaxValue);
        Task<bool> SaveAll();
        Task<Collection> GetCollection(int collectionId);
        Task<bool> IsUserCollectionExistAsync(int collectionId, int userId);
        Task<PagedList<User>> GetUsers(UserParams filters);
        Task<User> GetUser(int id);
        Task<Card> GetCardById(int cardId);
        Task<PagedList<Card>> GetCards(CardParams filters);
        Task<PagedList<ReadCard>> GetCardsToRead(int readCollectionId, CardParams filters);
        Task<IEnumerable<Card>> GetCard(int collectionId);
    }
}
