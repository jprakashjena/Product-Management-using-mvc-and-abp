using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace Volo.Abp.Account.Web.ExtensionGrants;

public abstract class TokenExtensionGrantBase : ITokenExtensionGrant
{
    protected virtual Task<Guid?> GetRawValueOrNullAsync(OpenIddictRequest request, string key)
    {
        var str = request.GetParameter(key).ToString();
        if (str.IsNullOrWhiteSpace())
        {
            return Task.FromResult<Guid?>(null);
        }

        return Guid.TryParse(str, out var guid) ? Task.FromResult<Guid?>(guid) : Task.FromResult<Guid?>(null);
    }

    protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ExtensionGrantContext context, ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        await foreach (var resource in context.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>().ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }

    protected virtual async Task SetClaimsDestinationsAsync(ExtensionGrantContext context, ClaimsPrincipal principal)
    {
        await context.HttpContext.RequestServices.GetRequiredService<AbpOpenIddictClaimDestinationsManager>().SetAsync(principal);
    }

    public abstract Task<IActionResult> HandleAsync(ExtensionGrantContext context);

    public abstract string Name { get; }
}
