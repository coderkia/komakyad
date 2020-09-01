using Kia.KomakYad.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Helpers
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(CustomClaimTypes.CardLimit, user.CardLimit?.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.CollectionLimit, user.CollectionLimit?.ToString()));
            return identity;
        }
    }
}
