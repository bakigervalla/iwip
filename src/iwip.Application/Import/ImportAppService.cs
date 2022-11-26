using iwip.MongoDb.Interfaces;
using iwip.PO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace iwip.Import
{
    public class ImportAppService : ApplicationService, IImportAppService
    {
        private readonly IImportRepository _importRepository;
        private readonly IRepository<PurchaseOrder, Guid> _poRepository;
        private readonly IRepository<Shipping, Guid> _shippingRepository;

        public ImportAppService(
                IImportRepository importRepository,
                IRepository<PurchaseOrder, Guid> poRepository, 
                IRepository<Shipping, Guid> shippingRepository
            )
        {
            _importRepository = importRepository;
            _poRepository = poRepository;
            _shippingRepository = shippingRepository;
        }

        public async Task ImportAsync(string collectionName, byte[] content)
        {

            string text = Encoding.ASCII.GetString(content);

            await _importRepository.ImportManyAsync(collectionName, text);
        }

        public async Task ImportPOAsync(List<PurchaseOrderDto> items)
        {
            try
            {
                var pos = ObjectMapper.Map<IEnumerable<PurchaseOrderDto>, IEnumerable<PurchaseOrder>>(items);
                await _poRepository.InsertManyAsync(pos, true);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                throw;
            }
        }

        public async Task ImportShippingAsync(List<ShippingDto> items)
        {
            try
            {
                var shippings = ObjectMapper.Map<IEnumerable<ShippingDto>, IEnumerable<Shipping>>(items);
                await _shippingRepository.InsertManyAsync(shippings, true);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                throw;
            }
        }



        private Guid IntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

    }
}
