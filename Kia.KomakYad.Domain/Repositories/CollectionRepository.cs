using Kia.KomakYad.DataAccess;
using Kia.KomakYad.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public class CollectionRepository : GenericRepository<Collection>, ICollectionRespository
    {
        public CollectionRepository(DataContext context) : base(context)
        {
        }

        public async Task<int> GetCollectionsCardsCount(int collectionId) =>
            await Find(c => c.Id == collectionId).Select(c => c.Cards).CountAsync();

        public async Task<int> GetFollowersCount(int collectionId)
        {
            return await context.ReadCollections.CountAsync(c => c.CollectionId == collectionId);
        }

        public async Task<int> GetUsersCollectionsCount(int userId) =>
            await Find(c => c.AuthorId == userId).CountAsync();

    }
}
