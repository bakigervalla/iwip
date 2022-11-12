using AutoMapper.Internal.Mappers;
using iwip.PO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace iwip.Blazor.Pages.PO.Shipping
{
    public partial class Index
    {

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string LineId { get; set; }

        [Inject]
        private IPOAppService POAppService { get; set; }

        [Inject]
        private IObjectMapper Mapper { get; set; }
        
        private bool loading { get; set; }

        private PurchaseOrderDto PurchaseOrder { get; set; } = new();
        private ShippingDto Shipping { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetShippingAsync();
        }

        private async Task GetShippingAsync()
        {
            loading = true;

            PurchaseOrder = await POAppService.GetPOAsync(new Guid(Id));

            var line = PurchaseOrder.PO_LINES.SingleOrDefault(l => l.PO_LINE_ID == int.Parse(LineId));

            Shipping = line?.SHIPPING;
            
            loading = false;
            StateHasChanged();
        }

        private async Task Save()
        {
            //var updatedPO = Mapper.Map<PurchaseOrderDto, CreateUpdatePODto>(PO);
            //await POAppService.UpdateAsync(updatedPO);
        }

        private async Task Create()
        {
            //var updatedPO = Mapper.Map<PurchaseOrderDto, CreateUpdatePODto>(PO);
            //await POAppService.CreateAsync(updatedPO);
        }

    }
}
