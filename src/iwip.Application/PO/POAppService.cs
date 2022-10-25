using iWipCloud.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static System.Net.Mime.MediaTypeNames;

namespace iwip.PO
{
    public class POAppService : ApplicationService, IPOAppService
    {
        private readonly IRepository<PurchaseOrder, Guid> _poRepository;

        public POAppService(IRepository<PurchaseOrder, Guid> poRepository)
        {
            _poRepository = poRepository;

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

        public async Task<PurchaseOrderDto> UpdateAsync(CreateUpdatePODto updatePO)
        {
            var poItem = await _poRepository.UpdateAsync(
                         new PurchaseOrder { POSTAL_CODE = updatePO.POSTAL_CODE }
                     );

            return ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(poItem);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _poRepository.DeleteAsync(id);
        }

        public async Task<List<PurchaseOrderDto>> GetListAsync()
        {
            var items = await _poRepository.GetListAsync();

            return ObjectMapper.Map<List<PurchaseOrder>, List<PurchaseOrderDto>>(items);
            //return items
            //    .Select(item => new PurchaseOrderDto
            //    {
            //        Id = item.Id,
            //        POSTAL_CODE = item.POSTAL_CODE
            //    }).ToList();
        }

    }
}
