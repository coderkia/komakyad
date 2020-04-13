using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess;
using Kia.KomakYad.Domain.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PagedList<UserWithRolesDto>> GetUsersWithRoles(UserWithRolesParams filters)
        {
            var userList = (from user in _context.Users
                                  orderby user.UserName
                                  select new UserWithRolesDto
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Roles = (from userRole in user.UserRoles
                                               join role in _context.Roles
                                               on userRole.RoleId
                                               equals role.Id
                                               select role.Name).ToList()
                                  }).AsQueryable();

            return await PagedList<UserWithRolesDto>.CreateAsync(userList, filters.PageNumber, filters.PageSize);
                                
        }
    }
}
