using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Volo.Abp.Identity;
public class UserPasswordChangeRequestedEventHandler :
    IDistributedEventHandler<UserPasswordChangeRequestedEto>,
    ITransientDependency
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IdentityUserManager IdentityUserManager { get; }

    public UserPasswordChangeRequestedEventHandler(
        IIdentityUserRepository userRepository,
        IdentityUserManager identityUserManager)
    {
        UserRepository = userRepository;
        IdentityUserManager = identityUserManager;
    }

    public async Task HandleEventAsync(UserPasswordChangeRequestedEto eventData)
    {
        if (!eventData.Password.IsNullOrEmpty())
        {
            var user = await UserRepository.FindByTenantIdAndUserNameAsync(eventData.UserName, eventData.TenantId);
            if (user != null)
            {
                (await IdentityUserManager.RemovePasswordAsync(user)).CheckErrors();
                (await IdentityUserManager.AddPasswordAsync(user, eventData.Password)).CheckErrors();
            }
        }
    }
}
