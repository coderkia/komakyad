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
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<PagedList<Collection>> GetCollections(CollectionParams filters);
        Task<PagedList<Collection>> GetUserCollections(int ownerId, CollectionParams filters);
        Task<int> GetDueCardCount(int readCollectionId, ReadCardParams filters);
        Task<int> GetFailedCount(int readCollectionId, ReadCardParams filters);
        Task<int> GeSucceedCount(int readCollectionId, ReadCardParams filters);
        Task<bool> SaveAll();
        Task<Collection> GetCollection(int collectionId);
        Task<bool> IsUserCollectionExistAsync(int collectionId, int userId);
        Task<PagedList<User>> GetUsers(UserParams filters);
        Task<User> GetUser(int id);
        Task<Card> GetCardById(int cardId);
        Task<PagedList<Card>> GetCards(CardParams filters);
        Task<PagedList<ReadCard>> GetReadCards(int readCollectionId, ReadCardParams filters);
        Task<IEnumerable<Card>> GetCards(int collectionId);
        Task<ReadCollection> GetReadCollection(int readCollectionId);
        Task<ReadCard> GetReadCard(int readcardId);
    }
}
