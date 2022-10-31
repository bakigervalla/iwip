using System;
using System.Collections.Generic;
using System.Text;

namespace iwip.PO
{
    public static class POPermissions
    {
        public const string GroupName = "PO";

        public static class PO
        {
            public const string Default = GroupName + ".PO";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }

    public static class ImportPermissions
    {
        public const string GroupName = "Import";

        public static class Import
        {
            public const string Default = GroupName + ".Import";
            public const string Create = Default + ".Import";
        }
    }

}
