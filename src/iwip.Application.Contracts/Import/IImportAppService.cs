using iwip.PO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace iwip.Import
{
    public interface IImportAppService : IApplicationService
    {
        Task<bool> UploadFileChunk(FileChunkDto fileChunkDto);
        Task<List<string>> GetFileNames();

        Task ImportAsync(ReadOnlyCollection<PurchaseOrderDto> purchaseOrders);
    }
}
