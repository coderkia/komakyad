using Kia.KomakYad.DataAccess;
using Kia.KomakYad.DataAccess.Models;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private DataContext _context;

        public AdminRepository(DataContext context)
        {
            _context = context;
        }
        public async Task SetCardLimit(User user, int limit)
        {
            user.CardLimit = limit;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task SetCollectionLimit(User user, int limit)
        {
            user.CollectionLimit = limit;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
