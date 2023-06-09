﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Users;

namespace Volo.Abp.Account;

[Authorize]
public class ProfileAppService : ApplicationService, IProfileAppService
{
    protected IdentityUserManager UserManager { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IdentityProTwoFactorManager IdentityProTwoFactorManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }


    public ProfileAppService(IdentityUserManager userManager,
        IdentitySecurityLogManager identitySecurityLogManager,
        IdentityProTwoFactorManager identityProTwoFactorManager,
        IOptions<IdentityOptions> identityOptions)
    {
        UserManager = userManager;
        IdentitySecurityLogManager = identitySecurityLogManager;
        IdentityProTwoFactorManager = identityProTwoFactorManager;
        IdentityOptions = identityOptions;
    }

    public virtual async Task<ProfileDto> GetAsync()
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

        return ObjectMapper.Map<IdentityUser, ProfileDto>(currentUser);
    }

    public virtual async Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        if (!string.Equals(user.UserName, input.UserName, StringComparison.InvariantCultureIgnoreCase))
        {
            if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled))
            {
                (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.ChangeUserName
                });
            }
        }

        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled))
            {
                (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.ChangeEmail
                });
            }
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.ChangePhoneNumber
            });
        }

        user.Name = input.Name;
        user.Surname = input.Surname;

        input.MapExtraPropertiesTo(user);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, ProfileDto>(user);
    }

    public virtual async Task ChangePasswordAsync(ChangePasswordInput input)
    {
        await IdentityOptions.SetAsync();

        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

        if (currentUser.IsExternal)
        {
            throw new BusinessException(code: IdentityErrorCodes.ExternalUserPasswordChange);
        }

        if (currentUser.PasswordHash == null)
        {
            (await UserManager.AddPasswordAsync(currentUser, input.NewPassword)).CheckErrors();
        }
        else
        {
            (await UserManager.ChangePasswordAsync(currentUser, input.CurrentPassword, input.NewPassword)).CheckErrors();
        }

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePassword
        });
    }

    public virtual async Task<bool> GetTwoFactorEnabledAsync()
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
        return await UserManager.GetTwoFactorEnabledAsync(currentUser);
    }

    public virtual async Task SetTwoFactorEnabledAsync(bool enabled)
    {
        if (await IdentityProTwoFactorManager.IsOptionalAsync())
        {
            if (await SettingProvider.GetAsync<bool>(IdentityProSettingNames.TwoFactor.UsersCanChange))
            {
                var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
                if (currentUser.TwoFactorEnabled != enabled)
                {
                    (await UserManager.SetTwoFactorEnabledAsync(currentUser, enabled)).CheckErrors();
                }
            }
            else
            {
                throw new BusinessException(code: IdentityErrorCodes.UsersCanNotChangeTwoFactor);
            }
        }
        else
        {
            throw new BusinessException(code: IdentityErrorCodes.CanNotChangeTwoFactor);
        }
    }
}
