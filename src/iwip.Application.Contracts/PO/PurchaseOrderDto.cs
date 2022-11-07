﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace iwip.PO
{
    public class PurchaseOrderDto
    {
        public PurchaseOrderDto()
        {
            SHIPPING = new List<ShippingDto>();
        }

        public Guid Id { get; set; }
        public int MANUFACTURER { get; set; }
        public int PO_HEADER_ID { get; set; }
        public string PURCHASE_ORDER_NUMBER { get; set; }
        public int REVISION_NUM { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [JsonIgnore]
        public DateTime REVISED_DATE { get; set; }
        [JsonIgnore]
        public DateTime PO_DATE { get; set; }
        public string PO_STATUS { get; set; }  // Status
        public string TYPE_LOOKUP_CODE { get; set; } // TypeCode
        public DateTime? CREATION_DATE { get; set; }
        [JsonIgnore]
        public DateTime LAST_UPDATE_DATE { get; set; }
        //public int LAST_UPDATED_BY { get; set; }
        public string SUMMARY_FLAG { get; set; }
        public string ENABLED_FLAG { get; set; }
        public string FREIGHT_TERMS_LOOKUP_CODE { get; set; }
        public string AUTHORIZATION_STATUS { get; set; }  // AuthorizationStatus
        public string APPROVED_FLAG { get; set; }
        [JsonIgnore]
        public bool APPROVED { get => APPROVED_FLAG?.ToLower() == "y"; }

        [JsonIgnore]
        public DateTime APPROVED_DATE { get; set; }
        public string CANCEL_FLAG { get; set; }
        public string CLOSED_CODE { get; set; }   // ClosedCode 
        public string VENDOR_NAME { get; set; }
        public int PARTY_ID { get; set; }
        public string PARTY_NUMBER { get; set; }
        public string VENDOR_NUMBER { get; set; }
        public string POSTAL_CODE { get; set; }

        // Collections
        // Collections
        public List<ShippingDto> SHIPPING { get; set; }

        public List<POLineDto> PO_LINES { get; set; }
    }
}
