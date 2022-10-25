using Volo.Abp.Settings;

namespace iwip.Settings;

public class iwipSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(iwipSettings.MySetting1));
    }
}
