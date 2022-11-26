using iwip.PO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace iwip.Import
{
    public interface IImportAppService : IApplicationService
    {
        Task ImportAsync(string collectionName, byte[] content);
        Task ImportPOAsync(List<PurchaseOrderDto> items);
        Task ImportShippingAsync(List<ShippingDto> items);
    }
}
