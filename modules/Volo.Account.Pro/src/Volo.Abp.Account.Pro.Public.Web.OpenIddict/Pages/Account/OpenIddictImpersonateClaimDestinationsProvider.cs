using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictImpersonateClaimDestinationsProvider : IAbpOpenIddictClaimDestinationsProvider, ITransientDependency
{
    public virtual Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context)
    {
        foreach (var claim in context.Claims)
        {
            if (claim.Type == AbpClaimTypes.ImpersonatorTenantId ||
                claim.Type == AbpClaimTypes.ImpersonatorTenantName ||
                claim.Type == AbpClaimTypes.ImpersonatorUserId ||
                claim.Type == AbpClaimTypes.ImpersonatorUserName)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
            }
        }

        return Task.CompletedTask;
    }
}
