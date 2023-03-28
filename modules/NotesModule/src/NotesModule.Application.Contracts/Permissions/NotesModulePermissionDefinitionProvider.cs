using NotesModule.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NotesModule.Permissions;

public class NotesModulePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NotesModulePermissions.GroupName, L("Permission:NotesModule"));

        var notePermission = myGroup.AddPermission(NotesModulePermissions.Notes.Default, L("Permission:Notes"));
        notePermission.AddChild(NotesModulePermissions.Notes.Create, L("Permission:Create"));
        notePermission.AddChild(NotesModulePermissions.Notes.Edit, L("Permission:Edit"));
        notePermission.AddChild(NotesModulePermissions.Notes.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NotesModuleResource>(name);
    }
}