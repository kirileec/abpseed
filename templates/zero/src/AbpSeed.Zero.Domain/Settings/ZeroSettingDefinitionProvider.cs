using Volo.Abp.Settings;

namespace AbpSeed.Zero.Settings;

public class ZeroSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ZeroSettings.MySetting1));
    }
}
