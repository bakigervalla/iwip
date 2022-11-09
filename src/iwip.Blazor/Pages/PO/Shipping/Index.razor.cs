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

        private ShippingDto Shipping { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetShippingAsync();
        }

        private async Task GetShippingAsync()
        {
            loading = true;

            Shipping = await POAppService.GetShippingAsync(new Guid(Id), int.Parse(LineId));
            
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
