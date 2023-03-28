using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.Account.Pro.Public.Blazor.Server;

public class AbpAccountBlazorBundleContributor: BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Volo.Abp.Account.Pro.Public.Blazor.Shared/libs/account/link-user.js");
    }
}
