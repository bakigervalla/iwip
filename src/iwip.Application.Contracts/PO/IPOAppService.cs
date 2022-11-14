using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace iwip.PO
{
    public interface IPOAppService : IApplicationService
    {
        Task<List<PurchaseOrderDto>> GetListAsync(bool disableTenant);
        Task<PurchaseOrderDto> CreateAsync(CreateUpdatePODto createPO);
        Task<PurchaseOrderDto> UpdateAsync(PurchaseOrderDto updatePO);
        Task DeleteAsync(Guid id);

        Task<PurchaseOrderDto> GetPOAsync(Guid id);

        // Shipping
        Task<ShippingDto> GetShippingAsync(int lineId, bool disableTenant);
        Task InsertShippingDocument(int lineId, ShippingDto shipping);
    }
}
