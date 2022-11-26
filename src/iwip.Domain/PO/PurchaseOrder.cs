using iwip.Helpers.Extensions;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MultiTenancy;

namespace iwip.PO
{
    /*[BsonIgnoreExtraElements]*/
    public class PurchaseOrder : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public PurchaseOrder()
        {
            this.CREATION_DATE = DateTime.Now;
        }

        //[BsonRepresentation(MongoDB.Bson.BsonType.Binary)]
        public Guid? TenantId { get; set; }

        //public override Guid Id { get; protected set; }
        public int MANUFACTURER { get; set; }
        public int PO_HEADER_ID { get; set; }
        public string PURCHASE_ORDER_NUMBER { get; set; }
        public int REVISION_NUM { get; set; }
        // [BsonElement]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        
        public DateTime? REVISED_DATE { get; set; }
        
        public DateTime? PO_DATE { get; set; }
        public string PO_STATUS { get; set; }  // Status
        public string TYPE_LOOKUP_CODE { get; set; } // TypeCode
        
        public DateTime? CREATION_DATE { get; set; }
        
        public DateTime? LAST_UPDATE_DATE { get; set; }

        [BsonIgnore]
        public Int32 LAST_UPDATED_BY { get; set; }
        public string SUMMARY_FLAG { get; set; }
        public string ENABLED_FLAG { get; set; }
        public string FREIGHT_TERMS_LOOKUP_CODE { get; set; }
        public string AUTHORIZATION_STATUS { get; set; }  // AuthorizationStatus

        public string APPROVED_FLAG { get; set; }
        [BsonIgnore]
        public bool APPROVED { get => APPROVED_FLAG?.ToLower() == "y"; }

        public DateTime? APPROVED_DATE { get; set; }
        public string CANCEL_FLAG { get; set; }
        public string CLOSED_CODE { get; set; }   // ClosedCode 
        public string VENDOR_NAME { get; set; }
        public int PARTY_ID { get; set; }
        public string PARTY_NUMBER { get; set; }
        public string VENDOR_NUMBER { get; set; }
        public string POSTAL_CODE { get; set; }

        // Collections
        //[BsonRepresentation(MongoDB.Bson.BsonType.Array)]
        public List<POLine> PO_LINES { get; set; }

    }
}