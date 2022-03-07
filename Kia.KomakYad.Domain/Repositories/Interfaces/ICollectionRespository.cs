using Kia.KomakYad.DataAccess.Models;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface ICollectionRespository: IRepository<Collection>
    {
        Task<int> GetUsersCollectionsCount(int userId);
        Task<int> GetCollectionsCardsCount(int collectionId);
        Task<int> GetFollowersCount(int collectionId);
    }
}
