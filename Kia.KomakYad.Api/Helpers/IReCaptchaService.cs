using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Helpers
{
    public interface IReCaptchaService
    {
        Task<bool> Validate(string token);
    }
}
