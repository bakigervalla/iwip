using iwip.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace iwip.PO
{
    public class POAppService : ApplicationService, IPOAppService
    {
        private readonly IRepository<PurchaseOrder, Guid> _poRepository;
        private readonly IRepository<Shipping, Guid> _shippingRepository;
        private readonly IMongoDbContextProvider<iwipMongoDbContext> _context;
        private readonly IDataFilter _dataFilter;

        public POAppService(
             IRepository<PurchaseOrder, Guid> poRepository
            , IRepository<Shipping, Guid> shippingRepository
            , IMongoDbContextProvider<iwipMongoDbContext> context
            , IDataFilter dataFilter
            , IObjectMapper objectMapper)
        {
            _poRepository = poRepository;
            _shippingRepository = shippingRepository;
            _dataFilter = dataFilter;
            _context = context;

            //GetPolicyName = POPermissions.PO.Default;
            //GetListPolicyName = POPermissions.PO.Default;
            //CreatePolicyName = POPermissions.PO.Create;
            //UpdatePolicyName = POPermissions.PO.Edit;
            //DeletePolicyName = POPermissions.PO.Delete;
        }

        public async Task<PurchaseOrderDto> CreateAsync(CreateUpdatePODto item)
        {
            var poItem = await _poRepository.InsertAsync(
                          new PurchaseOrder { POSTAL_CODE = item.POSTAL_CODE }
                      );

            return new PurchaseOrderDto
            {
                Id = poItem.Id,
                POSTAL_CODE = poItem.POSTAL_CODE,
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _poRepository.DeleteAsync(id);
        }

        public async Task<List<PurchaseOrderDto>> GetListAsync(bool disableTenant)
        {
            List<PurchaseOrder> purchaseOrders;
            if (disableTenant)
                using (_dataFilter.Disable<IMultiTenant>())
                {
                    purchaseOrders = await _poRepository.GetListAsync();
                }
            else
                purchaseOrders = await _poRepository.GetListAsync();

            return ObjectMapper.Map<List<PurchaseOrder>, List<PurchaseOrderDto>>(purchaseOrders);

            //return items
            //    .Select(item => new PurchaseOrderDto
            //    {
            //        Id = item.Id,
            //        POSTAL_CODE = item.POSTAL_CODE
            //    }).ToList();
        }

        public async Task<PurchaseOrderDto> GetPOAsync(Guid id)
        {
            try
            {
                var po = await _poRepository.GetAsync(id);

                return ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(po);
            }
            catch (Exception ex)
            {
                string sera = ex.Message;
                return null;
            }

            //return items
            //    .Select(item => new PurchaseOrderDto
            //    {
            //        Id = item.Id,
            //        POSTAL_CODE = item.POSTAL_CODE
            //    }).ToList();
        }

        public Task<PurchaseOrderDto> UpdateAsync(PurchaseOrderDto updatePO)
        {
            return null;
        }


        // Shipping
        public async Task<ShippingDto> GetShippingAsync(int lineId, bool disableTenant)
        {
            try
            {
                Expression<Func<Shipping, bool>> query = x => x.PO_LINE_ID == lineId;

                Shipping shipping;
                if (disableTenant)
                    using (_dataFilter.Disable<IMultiTenant>())
                    {
                        shipping = await _shippingRepository.GetAsync(query);
                    }
                else
                    shipping = await _shippingRepository.GetAsync(query);

                return ObjectMapper.Map<Shipping, ShippingDto>(shipping);

                /*
                    var line = queryable.Where(q => q.Id == id).SelectMany(l => l.PO_LINES);
                    var shipping = line?.FirstOrDefault(c => c.PO_LINE_ID == lineId)?.SHIPPING;
                    return ObjectMapper.Map<Shipping, ShippingDto>(shipping);
                */
            }
            catch (Exception ex)
            {
                string sera = ex.Message;
                return null;
            }
        }

        public async Task UpdateShipping(int lineId, ShippingDto shipping)
        {
            var update = ObjectMapper.Map<ShippingDto, Shipping>(shipping);
            await _shippingRepository.UpdateAsync(update);
        }

    }
}
