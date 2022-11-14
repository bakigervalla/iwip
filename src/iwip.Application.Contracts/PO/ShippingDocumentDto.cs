using System;
using System.Collections.Generic;
using System.Text;

namespace iwip.PO
{
    public class ShippingDocumentDto
    {
        public string FILE_NAME { get; set; }
        public string MIME_TYPE { get; set; }
        public string CONTENT { get; set; }
        public Guid? CREATED_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; } = DateTime.Now;
    }
}
