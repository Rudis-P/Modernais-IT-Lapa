using asp.net_Veikals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public CustomClaimsPrincipalFactory(
        UserManager<User> userManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("IsAdmin", user.IsAdmin.ToString()));
        return identity;
    }
}