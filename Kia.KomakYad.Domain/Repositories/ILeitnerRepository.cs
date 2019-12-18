using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface ILeitnerRepository
    {
        void Update<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<PagedList<Collection>> GetCollectionsAsync( CollectionParams collectionParams);
        IQueryable<DueCard> GetDueCards(int collectionId, int userId, byte deck = byte.MaxValue);
        Task<int> GetDueCardCount(int collectionId, int userId, byte deck = byte.MaxValue);
        Task<int> GetFailedCount(int collectionId, int userId, byte deck = byte.MaxValue);
        Task<int> GeSucceedCount(int collectionId, int userId, byte deck = byte.MaxValue);
        IQueryable<DueCard> GetReadCards(int collectionId, int userId, byte deck = byte.MaxValue);
        Task<bool> SaveAll();
        Task<Collection> GetCollection(int collectionId);
        Task<bool> IsUserCollectionExistAsync(int collectionId, int userId);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Card> GetCardById(int cardId);
    }
}
