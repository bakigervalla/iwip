using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iwip.PO
{
    // [BsonIgnoreExtraElements]
    public class ShippingDocument
    {
        public string FILE_NAME { get; set; }
        public string MIME_TYPE { get; set; }
        public string CONTENT { get; set; }
        public Guid? CREATED_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; } = DateTime.Now;
    }
}
