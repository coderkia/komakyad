using Kia.KomakYad.DataAccess.Models;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
