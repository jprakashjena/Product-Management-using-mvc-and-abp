﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityOptions_Tests : AbpIdentityDomainTestBase
{
    private ISettingProvider _settingProvider;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _settingProvider = Substitute.For<ISettingProvider>();
        _settingProvider.GetOrNullAsync(Arg.Any<string>()).Returns((string)null);
        services.Replace(ServiceDescriptor.Singleton(_settingProvider));
    }

    [Fact]
    public void Should_Resolve_AbpIdentityOptionsManager()
    {
        GetRequiredService<IOptions<IdentityOptions>>().ShouldBeOfType(typeof(AbpIdentityOptionsManager));
    }

    [Fact]
    public async Task Should_Get_Options_From_Custom_Settings_If_Available()
    {
        using (var scope1 = ServiceProvider.CreateScope())
        {
            var options = scope1.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;
            await (scope1.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>()).SetAsync();
            options.Password.RequiredLength.ShouldBe(6); //Default value
            options.Password.RequiredUniqueChars.ShouldBe(1); //Default value
        }

        _settingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequiredLength).Returns(Task.FromResult("42"));

        using (var scope2 = ServiceProvider.CreateScope())
        {
            var options = scope2.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;
            await (scope2.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>()).SetAsync();
            options.Password.RequiredLength.ShouldBe(42); //Setting value
            options.Password.RequiredUniqueChars.ShouldBe(1); //Default value
        }
    }
}
