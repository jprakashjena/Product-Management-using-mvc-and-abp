﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(ImpersonateTenantModel))]
public class OpenIddictImpersonateTenantModel : ImpersonateTenantModel
{
    protected readonly AbpAccountOpenIddictOptions Options;

    public OpenIddictImpersonateTenantModel(
        IOptions<AbpAccountOptions> accountOptions,
        IPermissionChecker permissionChecker,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IOptions<AbpAccountOpenIddictOptions> options)
        : base(accountOptions, permissionChecker, currentPrincipalAccessor)
    {
        Options = options.Value;
    }

    public async override Task<IActionResult> OnGetAsync()
    {
        if (!Request.Query.ContainsKey("access_token"))
        {
            return await base.OnGetAsync();
        }

        var authenticateResult = await HttpContext.AuthenticateAsync(Options.ImpersonationAuthenticationScheme);
        if (authenticateResult.Succeeded)
        {
            using (CurrentPrincipalAccessor.Change(authenticateResult.Principal))
            {
                if (CurrentTenant.Id != null)
                {
                    throw new BusinessException("Volo.Account:ImpersonateTenantOnlyAvailableForHost");
                }

                if (AccountOptions.ImpersonationTenantPermission.IsNullOrWhiteSpace() ||
                    !await PermissionChecker.IsGrantedAsync(AccountOptions.ImpersonationTenantPermission))
                {
                    throw new BusinessException("Volo.Account:RequirePermissionToImpersonateTenant")
                        .WithData("PermissionName", AccountOptions.ImpersonationTenantPermission);
                }

                var currentUserId = CurrentUser.Id;
                var currentUserName = CurrentUser.UserName;
                using (CurrentTenant.Change(TenantId))
                {
                    var adminUser = await UserManager.FindByNameAsync(AccountOptions.TenantAdminUserName);
                    if (adminUser != null)
                    {
                        try
                        {
                            return await OpenIddictAuthorizeResponse.GenerateAuthorizeResponseAsync(HttpContext, adminUser, new []
                            {
                                new Claim(AbpClaimTypes.ImpersonatorUserId, currentUserId.ToString()),
                                new Claim(AbpClaimTypes.ImpersonatorUserName, currentUserName)
                            });
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                            throw new BusinessException("Volo.Account:RequirePermissionToImpersonateTenant")
                                .WithData("PermissionName", AccountOptions.ImpersonationTenantPermission);
                        }
                    }

                    throw new BusinessException("Volo.Account:ThereIsNoUserWithUserName")
                        .WithData("UserName", AccountOptions.TenantAdminUserName);
                }
            }
        }

        throw new BusinessException("Volo.Account:RequirePermissionToImpersonateTenant")
            .WithData("PermissionName", AccountOptions.ImpersonationTenantPermission);
    }
}
