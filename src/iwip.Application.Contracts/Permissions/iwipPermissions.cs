namespace iwip.Permissions;

public static class iwipPermissions
{
    public const string GroupName = "iwip";

    public static class PO
    {
        public const string Default = GroupName + ".PO";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
