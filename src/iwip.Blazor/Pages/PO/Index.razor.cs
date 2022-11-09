using iwip.PO;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iwip.Blazor.Pages.PO
{
    public partial class Index
    {
        [Inject]
        private IPOAppService POAppService { get; set; }

        private bool loading { get; set; }

        private List<PurchaseOrderDto> PurchaseOrders { get; set; } = new List<PurchaseOrderDto>();
        private CreateUpdatePODto NewPO { get; set; } = new CreateUpdatePODto();
        private CreateUpdatePODto EditingPODto { get; set; } = new CreateUpdatePODto();


        public string SelectedOrder{ get; set; }
        public int? RowIndex { get; set; } = 1003;
        public void RowSelecthandler(RowSelectEventArgs<PurchaseOrderDto> Args)
        {
            SelectedOrder = Args.Data.VENDOR_NAME;
            RowIndex = Args.Data.PO_HEADER_ID;
        }

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
            loading = true;

            try
            {
                PurchaseOrders = await POAppService.GetListAsync();
            }
            catch(Exception ex)
            {
                await Notify.Info(ex.Message + Environment.NewLine + ex.InnerException?.Message);
            }
            loading = false;
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
