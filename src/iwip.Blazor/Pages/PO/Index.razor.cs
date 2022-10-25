using iwip.PO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iwip.Blazor.Pages.PO
{
    public partial class Index
    {
        [Inject]
        private IPOAppService POAppService { get; set; }

        private List<PurchaseOrderDto> PurchaseOrders { get; set; } = new List<PurchaseOrderDto>();
        private int TotalCount { get; set; }
        private CreateUpdatePODto NewPO { get; set; } = new CreateUpdatePODto();
        private CreateUpdatePODto EditingPODto { get; set; } = new CreateUpdatePODto();

        public Index()
        {
            //CreatePolicyName = POPermissions.PO.Create;
            //UpdatePolicyName = POPermissions.PO.Edit;
            //DeletePolicyName = POPermissions.PO.Delete;
        }
        protected override async Task OnInitializedAsync()
        {
            await GetPOsAsync();
        }

        private async Task GetPOsAsync()
        {
            PurchaseOrders = await POAppService.GetListAsync();

            StateHasChanged();
        }

        private async Task CreateAsync(IDictionary<string, object> input)
        {
            NewPO.MANUFACTURER = (int)input.GetOrDefault("MANUFACTURER");
            NewPO.PO_STATUS= input.GetOrDefault("PO_STATUS").ToString();
            /*NewPO.Price = (float)input.GetOrDefault("Price");*/
            NewPO.PO_DATE = (DateTime)input.GetOrDefault("PO_DATE");

            var result = await POAppService.CreateAsync(NewPO);
            PurchaseOrders.Add(result);
            NewPO = null;
        }

        private async Task UpdateAsync(PurchaseOrderDto poDto, IDictionary<string, object> input)
        {
            EditingPODto.PO_STATUS = input.GetOrDefault("PO_STATUS") == null ? poDto.PO_STATUS : input.GetOrDefault("PO_STATUS").ToString();
            EditingPODto.MANUFACTURER = input.GetOrDefault("MANUFACTURER") == null ? poDto.MANUFACTURER : (int)input.GetOrDefault("MANUFACTURER");
            /*EditingPODto.Price = input.GetOrDefault("Price") == null ? poDto.Price : (float)input.GetOrDefault("Price");*/
            EditingPODto.PO_DATE = input.GetOrDefault("PO_DATE") == null ? poDto.PO_DATE : (DateTime)input.GetOrDefault("PO_DATE");

            await POAppService.UpdateAsync(EditingPODto);

            await GetPOsAsync();

            EditingPODto = null;
        }

        private async Task DeleteAsync(PurchaseOrderDto item)
        {
            var confirmMessage = L["PO:DeleteConfirmationMessage", $"{item.PURCHASE_ORDER_NUMBER} - {item.MANUFACTURER}"];
            if (!await Message.Confirm(confirmMessage))
                return;

            await POAppService.DeleteAsync(item.Id);
            await Notify.Info("Deleted the PO item.");
            PurchaseOrders.Remove(item);
        }
    }
}
