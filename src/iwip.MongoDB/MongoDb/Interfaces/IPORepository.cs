using iwip.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace iwip.MongoDb.Interfaces
{
    public interface IPORepository : IRepository
    {
        Task<ShippingDocument> InsertShippingDocument(Guid id, int lineId, ShippingDocument attachment);
    }
}
