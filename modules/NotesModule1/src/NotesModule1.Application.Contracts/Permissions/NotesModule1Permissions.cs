using Volo.Abp.Reflection;

namespace NotesModule1.Permissions;

public class NotesModule1Permissions
{
    public const string GroupName = "NotesModule1";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(NotesModule1Permissions));
    }
}
