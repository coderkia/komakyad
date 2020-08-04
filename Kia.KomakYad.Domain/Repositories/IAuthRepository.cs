using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface IAuthRepository
    {
        Task<PagedList<UserWithRolesDto>> GetUsersWithRoles(UserWithRolesParams filters);
        Task<List<Role>> GetRoles();
    }
}
