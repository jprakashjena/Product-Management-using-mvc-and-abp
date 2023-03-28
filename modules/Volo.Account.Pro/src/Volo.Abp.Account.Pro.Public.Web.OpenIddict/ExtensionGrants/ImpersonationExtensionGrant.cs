using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Localization;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace Volo.Abp.Account.Web.ExtensionGrants;

public class ImpersonationExtensionGrant : TokenExtensionGrantBase
{
    public const string ExtensionGrantName = "Impersonation";

    public override string Name => ExtensionGrantName;

    protected IPermissionChecker permissionChecker { get; set; }
    protected ICurrentTenant currentTenant { get; set; }
    protected ICurrentUser currentUser { get; set; }
    protected ICurrentPrincipalAccessor currentPrincipalAccessor { get; set; }
    protected IdentityUserManager userManager { get; set; }
    protected IdentitySecurityLogManager identitySecurityLogManager { get; set; }
    protected ILogger<ImpersonationExtensionGrant> logger { get; set; }
    protected AbpAccountOptions abpAccountOptions { get; set; }
    protected IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory { get; set; }
    protected IStringLocalizer<AccountResource> localizer { get; set; }

    [UnitOfWork]
    public override async Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        using (CultureHelper.Use("en"))
        {
            permissionChecker = context.HttpContext.RequestServices.GetRequiredService<IPermissionChecker>();
            currentTenant = context.HttpContext.RequestServices.GetRequiredService<ICurrentTenant>();
            currentUser = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
            currentPrincipalAccessor = context.HttpContext.RequestServices.GetRequiredService<ICurrentPrincipalAccessor>();
            userManager = context.HttpContext.RequestServices.GetRequiredService<IdentityUserManager>();
            identitySecurityLogManager = context.HttpContext.RequestServices.GetRequiredService<IdentitySecurityLogManager>();
            logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ImpersonationExtensionGrant>>();
            abpAccountOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpAccountOptions>>().Value;
            userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
            localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<AccountResource>>();

            var transaction = await context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
            transaction.EndpointType = OpenIddictServerEndpointType.Introspection;
            transaction.Request = new OpenIddictRequest
            {
                ClientId = context.Request.ClientId,
                ClientSecret = context.Request.ClientSecret,
                Token = context.Request.AccessToken
            };
            var notification = new OpenIddictServerEvents.ProcessAuthenticationContext(transaction);
            var dispatcher = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerDispatcher>();
            await dispatcher.DispatchAsync(notification);

            if (notification.IsRejected)
            {
                logger.LogError("Process authentication rejected");

                return new ForbidResult(
                    new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                    }));
            }

            var principal = notification.GenericTokenPrincipal;
            if (principal == null)
            {
                logger.LogError("Process authentication principal is null");

                return new ForbidResult(
                    new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:InvalidAccessToken"]
                    }));
            }

            var request = context.HttpContext.GetOpenIddictServerRequest();

            using (currentPrincipalAccessor.Change(principal.Claims))
            {
                var tenantId = await GetRawValueOrNullAsync(request, "TenantId");
                var userId = await GetRawValueOrNullAsync(request, "UserId");
                var impersonatorUserId = currentPrincipalAccessor.Principal.FindImpersonatorUserId();

                if (userId == null && tenantId == null && impersonatorUserId != null)
                {
                    return await BackToImpersonatorAsync(
                        context,
                        principal,
                        currentPrincipalAccessor.Principal.FindImpersonatorTenantId(),
                        impersonatorUserId.Value);
                }

                if (impersonatorUserId != null)
                {
                    return new ForbidResult(
                        new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:NestedImpersonationIsNotAllowed"]
                        }));
                }

                if (currentTenant.IsAvailable)
                {
                    //Tenant
                    if (userId != null)
                    {
                        return await ImpersonateUserAsync(context, principal, currentTenant.Id, userId.Value);
                    }
                }
                else
                {
                    //Host
                    if (userId == null && tenantId != null)
                    {
                        return await ImpersonateTenantAsync(context, principal, tenantId.Value);
                    }

                    if (userId != null && tenantId == null)
                    {
                        return await ImpersonateUserAsync(context, principal, null, userId.Value);
                    }
                }

                return new ForbidResult(
                    new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:InvalidTenantIdOrUserId"]
                    }));
            }
        }
    }

    protected virtual async Task<IActionResult> ImpersonateTenantAsync(ExtensionGrantContext context, ClaimsPrincipal principal, Guid tenantId)
    {
        if (abpAccountOptions.ImpersonationTenantPermission.IsNullOrWhiteSpace() ||
            await permissionChecker.IsGrantedAsync(abpAccountOptions.ImpersonationTenantPermission))
        {
            using (currentTenant.Change(tenantId))
            {
                var user = await userManager.FindByNameAsync(abpAccountOptions.TenantAdminUserName);
                if (user != null)
                {
                    var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);

                    var additionalClaims = new List<Claim>() {
                        new Claim(AbpClaimTypes.ImpersonatorUserId, currentUser.Id.ToString()),
                        new Claim(AbpClaimTypes.ImpersonatorUserName, currentUser.UserName)
                    };

                    claimsPrincipal.Identities.First().AddClaims(additionalClaims);

                    using (currentPrincipalAccessor.Change(claimsPrincipal))
                    {
                        await identitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext {
                            Identity = IdentitySecurityLogIdentityConsts.Identity, Action = "ImpersonateUser"
                        });
                    }

                    claimsPrincipal.SetScopes(principal.GetScopes());
                    claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
                    await SetClaimsDestinationsAsync(context, claimsPrincipal);

                    return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
                }

                logger.LogError(localizer["Volo.Account:ThereIsNoUserWithUserName"].ToString().Replace("{UserName}", abpAccountOptions.TenantAdminUserName));

                return new ForbidResult(
                    new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:ThereIsNoUserWithUserName"].ToString().Replace("{UserName}", abpAccountOptions.TenantAdminUserName)
                    }));
            }
        }

        logger.LogError( localizer["Volo.Account:RequirePermissionToImpersonateTenant"].ToString().Replace("{PermissionName}", abpAccountOptions.ImpersonationTenantPermission));

        return new ForbidResult(
            new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
            properties: new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:RequirePermissionToImpersonateTenant"].ToString().Replace("{PermissionName}", abpAccountOptions.ImpersonationTenantPermission)
            }));
    }

    protected virtual async Task<IActionResult> ImpersonateUserAsync(ExtensionGrantContext context, ClaimsPrincipal principal, Guid? tenantId, Guid userId)
    {
        if (userId == currentUser.Id)
        {
            logger.LogError(localizer["Volo.Account:YouCanNotImpersonateYourself"]);

            return new ForbidResult(
                new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:YouCanNotImpersonateYourself"]
                }));
        }

        if (abpAccountOptions.ImpersonationUserPermission.IsNullOrWhiteSpace() ||
            await permissionChecker.IsGrantedAsync(abpAccountOptions.ImpersonationUserPermission))
        {
            using (currentTenant.Change(tenantId))
            {
                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);

                    var additionalClaims = new List<Claim>();
                    if (currentUser.Id?.ToString() != currentUser.FindClaim(AbpClaimTypes.ImpersonatorUserId)?.Value)
                    {
                        additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorUserId, currentUser.Id.ToString()));
                        additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorUserName, currentUser.UserName));
                        if (currentTenant.IsAvailable)
                        {
                            additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorTenantId, currentTenant.Id.ToString()));
                            var tenantConfiguration = await context.HttpContext.RequestServices.GetRequiredService<ITenantStore>().FindAsync(currentTenant.Id.Value);
                            if (tenantConfiguration != null && !tenantConfiguration.Name.IsNullOrWhiteSpace())
                            {
                                additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorTenantName, tenantConfiguration.Name));
                            }
                        }
                    }

                    claimsPrincipal.Identities.First().AddClaims(additionalClaims);

                    using (currentPrincipalAccessor.Change(claimsPrincipal))
                    {
                        await identitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                        {
                            Identity = IdentitySecurityLogIdentityConsts.Identity,
                            Action = "ImpersonateUser"
                        });
                    }

                    claimsPrincipal.SetScopes(principal.GetScopes());
                    claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
                    await SetClaimsDestinationsAsync(context, claimsPrincipal);

                    return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
                }

                logger.LogError(localizer["Volo.Account:ThereIsNoUserWithId"].ToString().Replace("{UserId}", userId.ToString()));

                return new ForbidResult(
                    new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:ThereIsNoUserWithId"].ToString().Replace("{UserId}", userId.ToString())
                    }));
            }
        }

        logger.LogError(localizer["Volo.Account:RequirePermissionToImpersonateUser"].ToString().Replace("{PermissionName}", abpAccountOptions.ImpersonationUserPermission));

        return new ForbidResult(
            new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
            properties: new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:RequirePermissionToImpersonateUser"].ToString().Replace("{PermissionName}", abpAccountOptions.ImpersonationUserPermission)
            }));
    }

    protected virtual async Task<IActionResult> BackToImpersonatorAsync(ExtensionGrantContext context, ClaimsPrincipal principal, Guid? tenantId, Guid userId)
    {
        using (currentTenant.Change(tenantId))
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);

                claimsPrincipal.SetScopes(principal.GetScopes());
                claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
                await SetClaimsDestinationsAsync(context, claimsPrincipal);

                return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
            }

            return new ForbidResult(
                new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["Volo.Account:InvalidAccessToken"]
                }));
        }
    }
}
