using Volo.Abp.Settings;

namespace ProductManagementNew.Settings;

public class ProductManagementNewSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ProductManagementNewSettings.MySetting1));
    }
}
