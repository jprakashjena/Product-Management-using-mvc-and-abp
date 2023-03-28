using Volo.Abp.Reflection;

namespace NotesModule.Permissions;

public class NotesModulePermissions
{
    public const string GroupName = "NotesModule";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(NotesModulePermissions));
    }

    public static class Notes
    {
        public const string Default = GroupName + ".Notes";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}