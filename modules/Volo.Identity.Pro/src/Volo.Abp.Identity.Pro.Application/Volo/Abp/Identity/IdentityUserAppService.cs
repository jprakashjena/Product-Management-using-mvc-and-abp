﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity;

[Authorize(IdentityPermissions.Users.Default)]
public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
{
    protected IdentityUserManager UserManager { get; }
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityRoleRepository RoleRepository { get; }
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }
    protected IdentityProTwoFactorManager IdentityProTwoFactorManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IOptions<AbpIdentityOptions> AbpIdentityOptions { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IPermissionChecker PermissionChecker  { get; }

    public IdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOrganizationUnitRepository organizationUnitRepository,
        IIdentityClaimTypeRepository identityClaimTypeRepository,
        IdentityProTwoFactorManager identityProTwoFactorManager,
        IOptions<IdentityOptions> identityOptions,
        IDistributedEventBus distributedEventBus,
        IOptions<AbpIdentityOptions> abpIdentityOptions,
        IPermissionChecker permissionChecker)
    {
        UserManager = userManager;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        OrganizationUnitRepository = organizationUnitRepository;
        IdentityClaimTypeRepository = identityClaimTypeRepository;
        IdentityProTwoFactorManager = identityProTwoFactorManager;
        IdentityOptions = identityOptions;
        DistributedEventBus = distributedEventBus;
        AbpIdentityOptions = abpIdentityOptions;
        PermissionChecker = permissionChecker;
    }

    public virtual async Task<IdentityUserDto> GetAsync(Guid id)
    {
        var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await UserManager.GetByIdAsync(id)
        );

        userDto.SupportTwoFactor = await IdentityProTwoFactorManager.IsOptionalAsync();
        return userDto;
    }

    public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        var count = await UserRepository.GetCountAsync(
            input.Filter,
            input.RoleId,
            input.OrganizationUnitId,
            input.UserName,
            input.PhoneNumber,
            input.EmailAddress,
            input.IsLockedOut,
            input.NotActive
        );

        var users = await UserRepository.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.Filter,
            includeDetails: true,
            input.RoleId,
            input.OrganizationUnitId,
            input.UserName,
            input.PhoneNumber,
            input.EmailAddress,
            input.IsLockedOut,
            input.NotActive
        );

        var userRoleIds = users.SelectMany(x => x.Roles)
            .Select(x => x.RoleId)
            .Distinct()
            .ToList();

        var userRoles = await RoleRepository.GetListAsync(userRoleIds);

        var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(users);

        var twoFactorEnabled = await IdentityProTwoFactorManager.IsOptionalAsync();
        for (int i = 0; i < users.Count; i++)
        {
            userDtos[i].IsLockedOut = users[i].LockoutEnabled && (users[i].LockoutEnd != null && users[i].LockoutEnd > DateTime.UtcNow);
            if (!userDtos[i].IsLockedOut)
            {
                userDtos[i].LockoutEnd = null;
            }
            userDtos[i].SupportTwoFactor = twoFactorEnabled;
            userDtos[i].RoleNames = userRoles
                .Where(x => users[i].Roles.Any(q => q.RoleId == x.Id))
                .Select(x => x.Name).ToList();
        }

        return new PagedResultDto<IdentityUserDto>(
            count,
            userDtos
        );
    }

    public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        var roles = await UserRepository.GetRolesAsync(id);
        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
        );
    }

    public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        var list = await RoleRepository.GetListAsync();
        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
    }

    public virtual async Task<ListResultDto<OrganizationUnitWithDetailsDto>> GetAvailableOrganizationUnitsAsync()
    {
        var organizationUnits = await OrganizationUnitRepository.GetListAsync(includeDetails: true);
        var roleLookup = await GetRoleLookup(organizationUnits);
        var ouDtos = new List<OrganizationUnitWithDetailsDto>();
        foreach (var ou in organizationUnits)
        {
            ouDtos.Add(
                await ConvertToOrganizationUnitWithDetailsDtoAsync(ou, roleLookup)
            );
        }
        return new ListResultDto<OrganizationUnitWithDetailsDto>(ouDtos);
    }

    public virtual async Task<List<ClaimTypeDto>> GetAllClaimTypesAsync()
    {
        var claimTypes = await IdentityClaimTypeRepository.GetListAsync();

        var dtos = ObjectMapper.Map<List<IdentityClaimType>, List<ClaimTypeDto>>(claimTypes);
        foreach (var dto in dtos)
        {
            dto.ValueTypeAsString = dto.ValueType.ToString();
        }

        return dtos;
    }

    public virtual async Task<List<IdentityUserClaimDto>> GetClaimsAsync(Guid id)
    {
        var user = await UserRepository.GetAsync(id);
        return new List<IdentityUserClaimDto>(
            ObjectMapper.Map<List<IdentityUserClaim>, List<IdentityUserClaimDto>>(user.Claims.ToList())
        );
    }

    public virtual async Task<List<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
    {
        var organizationUnits = await UserRepository.GetOrganizationUnitsAsync(id, includeDetails: true);
        return new List<OrganizationUnitDto>(
            ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(organizationUnits)
        );
    }

    [Authorize(IdentityPermissions.Users.Create)]
    public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(
            GuidGenerator.Create(),
            input.UserName,
            input.Email,
            CurrentTenant.Id
        );

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        await UpdateUserByInput(user, input);
        (await UserManager.UpdateAsync(user)).CheckErrors();
        await CurrentUnitOfWork.SaveChangesAsync();

        await DistributedEventBus.PublishAsync(new IdentityUserCreatedEto()
        {
            Id = user.Id,
            Properties =
                {
                    { "SendConfirmationEmail", input.SendConfirmationEmail.ToString().ToUpper() },
                    { "AppName", "MVC" }
                }
        });

        var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

        return userDto;
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
        await UpdateUserByInput(user, input);
        input.MapExtraPropertiesTo(user);
        (await UserManager.UpdateAsync(user)).CheckErrors();
        await CurrentUnitOfWork.SaveChangesAsync();

        var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

        return userDto;
    }

    [Authorize(IdentityPermissions.Users.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        if (CurrentUser.Id == id)
        {
            throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
        }

        var user = await UserManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return;
        }

        (await UserManager.DeleteAsync(user)).CheckErrors();
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        var user = await UserManager.GetByIdAsync(id);
        (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        await UserRepository.UpdateAsync(user);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdateClaimsAsync(Guid id, List<IdentityUserClaimDto> input)
    {
        var user = await UserRepository.GetAsync(id);

        foreach (var claim in input)
        {
            var existing = user.FindClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            if (existing == null)
            {
                user.AddClaim(GuidGenerator, new Claim(claim.ClaimType, claim.ClaimValue));
            }
        }

        //Copied with ToList to avoid modification of the collection in the for loop
        foreach (var claim in user.Claims.ToList())
        {
            if (!input.Any(c => claim.ClaimType == c.ClaimType && claim.ClaimValue == c.ClaimValue))
            {
                user.RemoveClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            }
        }

        await UserRepository.UpdateAsync(user);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public async Task LockAsync(Guid id, DateTime lockoutEnd)
    {
        var user = await UserManager.GetByIdAsync(id);
        if (!await UserManager.GetLockoutEnabledAsync(user))
        {
            throw new UserFriendlyException(L["UserLockoutNotEnabled{0}", user.UserName]);
        }
        lockoutEnd = DateTime.SpecifyKind(lockoutEnd, DateTimeKind.Utc);
        (await UserManager.SetLockoutEndDateAsync(user, lockoutEnd)).CheckErrors();
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UnlockAsync(Guid id)
    {
        var user = await UserManager.GetByIdAsync(id);
        if (!await UserManager.GetLockoutEnabledAsync(user))
        {
            throw new UserFriendlyException(L["UserLockoutNotEnabled{0}", user.UserName]);
        }

        (await UserManager.SetLockoutEndDateAsync(user, null)).CheckErrors();
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordInput input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);
        (await UserManager.RemovePasswordAsync(user)).CheckErrors();
        (await UserManager.AddPasswordAsync(user, input.NewPassword)).CheckErrors();
    }

    public virtual async Task<IdentityUserDto> FindByUsernameAsync(string username)
    {
        var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await UserManager.FindByNameAsync(username)
        );

        return userDto;
    }

    public virtual async Task<IdentityUserDto> FindByEmailAsync(string email)
    {
        var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await UserManager.FindByEmailAsync(email)
        );

        return userDto;
    }

    public virtual async Task<bool> GetTwoFactorEnabledAsync(Guid id)
    {
        var user = await UserManager.GetByIdAsync(id);
        return await UserManager.GetTwoFactorEnabledAsync(user);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task SetTwoFactorEnabledAsync(Guid id, bool enabled)
    {
        if (await IdentityProTwoFactorManager.IsOptionalAsync())
        {
            var user = await UserManager.GetByIdAsync(id);
            if (user.TwoFactorEnabled != enabled)
            {
                (await UserManager.SetTwoFactorEnabledAsync(user, enabled)).CheckErrors();
            }
        }
        else
        {
            throw new BusinessException(code: IdentityErrorCodes.CanNotChangeTwoFactor);
        }
    }

    public async Task<List<IdentityRoleLookupDto>> GetRoleLookupAsync()
    {
        var roles = await RoleRepository.GetListAsync();

        return ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleLookupDto>>(roles);
    }

    public async Task<List<OrganizationUnitLookupDto>> GetOrganizationUnitLookupAsync()
    {
        var organizationUnits = await OrganizationUnitRepository.GetListAsync();

        return ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitLookupDto>>(organizationUnits);
    }

    [Authorize(IdentityPermissions.Users.Import)]
    public virtual async Task<List<ExternalLoginProviderDto>> GetExternalLoginProvidersAsync()
    {
        var providers = new List<ExternalLoginProviderDto>();

        foreach (var externalLoginProvider in AbpIdentityOptions.Value.ExternalLoginProviders)
        {
            var provider = LazyServiceProvider.LazyGetRequiredService(externalLoginProvider.Value.Type).As<IExternalLoginProvider>();

            if (await provider.IsEnabledAsync())
            {
                var canObtainUserInfoWithoutPassword = true;
                if (provider is IExternalLoginProviderWithPassword providerWithPassword)
                {
                    canObtainUserInfoWithoutPassword = providerWithPassword.CanObtainUserInfoWithoutPassword;
                }

                providers.Add(new ExternalLoginProviderDto(externalLoginProvider.Value.Name, canObtainUserInfoWithoutPassword));
            }

        }

        return providers;
    }

    [Authorize(IdentityPermissions.Users.Import)]
    public virtual async Task<IdentityUserDto> ImportExternalUserAsync(ImportExternalUserInput input)
    {
        if (!AbpIdentityOptions.Value.ExternalLoginProviders.TryGetValue(input.Provider, out var providerInfo))
        {
            throw new BusinessException(IdentityProErrorCodes.InvalidExternalLoginProvider);
        }

        var provider = LazyServiceProvider.LazyGetRequiredService(providerInfo.Type).As<IExternalLoginProvider>();
        var user = await UserManager.FindByNameAsync(input.UserNameOrEmailAddress) ?? await UserManager.FindByEmailAsync(input.UserNameOrEmailAddress);

        if (provider is IExternalLoginProviderWithPassword)
        {
            if (!provider.As<IExternalLoginProviderWithPassword>().CanObtainUserInfoWithoutPassword && !await provider.TryAuthenticateAsync(input.UserNameOrEmailAddress, input.Password))
            {
                throw new BusinessException(IdentityProErrorCodes.ExternalLoginProviderAuthenticateFailed);
            }
        }

        if (user == null)
        {
            if (provider is IExternalLoginProviderWithPassword providerWithPassword)
            {
                user = await providerWithPassword.CreateUserAsync(input.UserNameOrEmailAddress, input.Provider, input.Password);
            }
            else
            {
                user = await provider.CreateUserAsync(input.UserNameOrEmailAddress, input.Provider);
            }
        }
        else
        {
            if (!user.IsExternal)
            {
                throw new BusinessException(IdentityProErrorCodes.LocalUserAlreadyExists);
            }

            if (provider is IExternalLoginProviderWithPassword providerWithPassword)
            {
                await providerWithPassword.UpdateUserAsync(user, input.Provider, input.Password);
            }
            else
            {
                await provider.UpdateUserAsync(user, input.Provider);
            }
        }

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
    {
        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

        (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

        user.Name = input.Name;
        user.Surname = input.Surname;
        (await UserManager.UpdateAsync(user)).CheckErrors();
        user.SetIsActive(input.IsActive);

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles) && input.RoleNames != null)
        {
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        }

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageOU) && input.OrganizationUnitIds != null)
        {
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnitIds);
        }
    }

    private async Task<OrganizationUnitWithDetailsDto> ConvertToOrganizationUnitWithDetailsDtoAsync(
        OrganizationUnit organizationUnit,
        Dictionary<Guid, IdentityRole> roleLookup
    )
    {
        var dto = ObjectMapper.Map<OrganizationUnit, OrganizationUnitWithDetailsDto>(organizationUnit);
        dto.Roles = new List<IdentityRoleDto>();
        foreach (var ouRole in organizationUnit.Roles)
        {
            var role = roleLookup.GetOrDefault(ouRole.RoleId);
            if (role != null)
            {
                dto.Roles.Add(ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role));
            }
        }

        return await Task.FromResult(dto);
    }

    private async Task<Dictionary<Guid, IdentityRole>> GetRoleLookup(IEnumerable<OrganizationUnit> organizationUnits)
    {
        var roleIds = organizationUnits
            .SelectMany(q => q.Roles)
            .Select(t => t.RoleId)
            .Distinct()
            .ToArray();

        return (await RoleRepository.GetListAsync(roleIds))
            .ToDictionary(u => u.Id, u => u);
    }
}
