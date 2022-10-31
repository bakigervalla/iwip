using iwip.Localization;
using iwip.PO;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
namespace iwip.Permissions;

public class iwipPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        /*var myGroup = context.AddGroup(iwipPermissions.GroupName);*/

        var poGroup = context.AddGroup(POPermissions.GroupName, L("Permission:PO"));

        var poPermission = poGroup.AddPermission(POPermissions.PO.Default, L("Permission:PO"));
        poPermission.AddChild(POPermissions.PO.Create, L("Permission:PO.Create"));
        poPermission.AddChild(POPermissions.PO.Edit, L("Permission:PO.Edit"));
        poPermission.AddChild(POPermissions.PO.Delete, L("Permission:PO.Delete"));

        // import
        var importGroup = context.AddGroup(ImportPermissions.GroupName, L("Permission:Import"));
        importGroup.AddPermission(ImportPermissions.Import.Default, L("Permission:Import"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<iwipResource>(name);
    }
}
