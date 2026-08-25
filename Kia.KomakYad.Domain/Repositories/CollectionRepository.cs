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

        public async Task<int> GetCardsCount(int collectionId) =>
            await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(Find(c => c.Id == collectionId).Select(c => c.Cards));

        public async Task<int> GetCollectionsCardsCount(int collectionId) =>
            await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(Find(c => c.Id == collectionId).Select(c => c.Cards));

        public async Task<int> GetFollowersCount(int collectionId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(context.ReadCollections, c => c.CollectionId == collectionId);
        }

        public async Task<int> GetUsersCollectionsCount(int userId) =>
            await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(Find(c => c.AuthorId == userId));

    }
}
