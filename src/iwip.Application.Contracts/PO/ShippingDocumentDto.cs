using System;
using System.Collections.Generic;
using System.Text;

namespace iwip.PO
{
    public class ShippingDocumentDto
    {
        public int MANUFACTURER { get; set; }
        public int PO_HEADER_ID { get; set; }
        public int PO_LINE_ID { get; set; }
        public byte[] FILE_CONTENT { get; set; }
        public int CREATED_BY { get; set; }

        public DateTime? CREATE_DATE { get; set; }
        public int UPDATED_BY { get; set; }

        public DateTime? UPDATE_DATE { get; set; }
    }
}
