using System;
using System.Collections.Generic;
using System.Text;

namespace iwip.PO
{
    public class POLineDto
    {
        public int MANUFACTURER { get; set; }
        public int PO_HEADER_ID { get; set; }
        public int PO_LINE_ID { get; set; }
        public int LINE_NUM { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string UNIT_MEAS_LOOKUP_CODE { get; set; }
        public string NOTE_TO_VENDOR { get; set; }
        public string SKU { get; set; }
        public int QUANTITY_REMAINING { get; set; }
        public DateTime EX_FACTORY_DATE { get; set; }
        public string PO_LINE_TYPE { get; set; }
        public int QUANTITY_ORDERED { get; set; }
        public int QUANTITY_DELIVERED { get; set; }
        public int QUANTITY_CANCELLED { get; set; }
    }
}
