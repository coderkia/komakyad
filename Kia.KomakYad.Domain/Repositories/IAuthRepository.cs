using Kia.KomakYad.Common.Helpers;
using Kia.KomakYad.Domain.Dtos;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface IAuthRepository
    {
        Task<PagedList<UserWithRolesDto>> GetUsersWithRoles(UserWithRolesParams filters);
    }
}
