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
}
