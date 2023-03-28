using NotesModule1.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NotesModule1.Permissions;

public class NotesModule1PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NotesModule1Permissions.GroupName, L("Permission:NotesModule1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NotesModule1Resource>(name);
    }
}
