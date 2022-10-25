using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace iwip.PO
{
    public class CreateUpdatePODto
    {
        public Guid Id { get; set; }
        public int MANUFACTURER { get; set; }
        public int PO_HEADER_ID { get; set; }
        public int PURCHASE_ORDER_NUMBER { get; set; }
        public int REVISION_NUM { get; set; }
        public DateTime REVISED_DATE { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PO_DATE { get; set; }
        [Required]
        public string PO_STATUS { get; set; }
        [Required]
        public TypeCode TYPE_LOOKUP_CODE { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public DateTime LAST_UPDATE_DATE { get; set; }
        public int LAST_UPDATED_BY { get; set; }

        [Required]
        [StringLength(128)]
        public string SUMMARY_FLAG { get; set; }
        public string ENABLED_FLAG { get; set; }
        public string FREIGHT_TERMS_LOOKUP_CODE { get; set; }
        public string AUTHORIZATION_STATUS { get; set; }
        public string APPROVED_FLAG { get; set; }
        public DateTime APPROVED_DATE { get; set; }
        public string CANCEL_FLAG { get; set; }
        public string CLOSED_CODE { get; set; }
        public string VENDOR_NAME { get; set; }
        public int PARTY_ID { get; set; }
        public int PARTY_NUMBER { get; set; }
        public int VENDOR_NUMBER { get; set; }
        public string POSTAL_CODE { get; set; }
    }
}
