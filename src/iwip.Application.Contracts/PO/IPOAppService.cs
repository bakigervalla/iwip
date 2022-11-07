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
        Task<List<PurchaseOrderDto>> GetListAsync();
        Task<PurchaseOrderDto> CreateAsync(CreateUpdatePODto createPO);
        Task<PurchaseOrderDto> UpdateAsync(CreateUpdatePODto updatePO);
        Task DeleteAsync(Guid id);

        Task<PurchaseOrderDto> GetPOAsync(Guid id);
    }
}
