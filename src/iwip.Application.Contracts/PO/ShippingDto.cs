using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iwip.PO
{
    public class ShippingDto
    {
        public ShippingDto()
        {
            SHIPPING_DOCUMENTS = new List<ShippingDocumentDto>();
        }
        public Guid? TenantId { get; set; }
        public Guid Id { get; set; }
        public int PO_HEADER_ID { get; set; }
        public int PO_LINE_ID { get; set; }
        public int MANUFACTURER { get; set; }
        public string SHIP_TO_LOCATION { get; set; }
        public string SHIP_TO_SITE_DESCRIPTION { get; set; }
        public int SHIP_TO_LOCATION_ID { get; set; }
        public string ADDRESS_STYLE { get; set; }
        public string ADDRESS_LINE_1 { get; set; }
        public string ADDRESS_LINE_2 { get; set; }
        public string ADDRESS_LINE_3 { get; set; }
        public string TOWN_OR_CITY { get; set; }
        public string COUNTRY { get; set; }
        public string POSTAL_CODE { get; set; }
        public string REGION_1 { get; set; }
        public string REGION_2 { get; set; }
        public string TELEPHONE_NUMBER_1 { get; set; }
        public string ATTRIBUTE1 { get; set; }
        public string ATTRIBUTE2 { get; set; }
        public string ATTRIBUTE3 { get; set; }
        public string DERIVED_LOCALE { get; set; }
        public string TIMEZONE_CODE { get; set; }
        
        public DateTime? NEED_BY_DATE { get; set; }
        public DateTime? PROMISED_DATE { get; set; }
        public string LINE_CLOSED_CODE { get; set; }
        public int LINE_LOCATION_ID { get; set; }
        public int ITEM_ID { get; set; }
        public int NON_Z_ITEM_ID { get; set; }
        public DateTime? BUILD_WEEK { get; set; }
        public string SHIP_TO_SITE_REGION { get; set; }
        public string LAST_UPDATE_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public string PO_ORDER_TYPE { get; set; }

        public List<ShippingDocumentDto> SHIPPING_DOCUMENTS { get; set; }
    }
}
