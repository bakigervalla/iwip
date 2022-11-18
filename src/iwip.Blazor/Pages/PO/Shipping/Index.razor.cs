using Autofac.Core;
using iwip.PO;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using System;
using System.Buffers.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.Users;
using static System.Net.WebRequestMethods;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;


namespace iwip.Blazor.Pages.PO.Shipping
{
    public partial class Index
    {
        [Parameter]
        public string LineId { get; set; }

        [Inject]
        private IPOAppService POAppService { get; set; }

        [Inject]
        private IObjectMapper Mapper { get; set; }

        [Inject]
        private ICurrentUser CurrentUser { get; set; }

        [Inject]
        private IUiNotificationService Notify { get; set; }

        [Inject]
        private IUiMessageService UIMessageService { get; set; }

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

            Shipping = await POAppService.GetShippingAsync(int.Parse(LineId), !CurrentUser.TenantId.HasValue);

            /*var line = PurchaseOrder.PO_LINES.SingleOrDefault(l => l.PO_LINE_ID == int.Parse(LineId));

            Shipping = line?.SHIPPING;*/

            loading = false;
            StateHasChanged();
        }

        private async Task UploadShippmentDocument(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                try
                {
                    var bytes = file.Stream.ToArray();
                    var base64 = Convert.ToBase64String(bytes);

                    Shipping.SHIPPING_DOCUMENTS.Add(new ShippingDocumentDto
                    {
                        FILE_NAME = file.FileInfo.Name,
                        MIME_TYPE = MimeTypes.GetMimeType(file.FileInfo.Type),
                        CONTENT = base64,
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = CurrentUser.Id
                    });

                    await POAppService.UpdateShipping(Shipping.PO_LINE_ID, Shipping);

                    await Notify.Info("Uploaded Successfully.");

                }
                catch (Exception ex)
                {
                    await Notify.Info(ex.Message);
                }
            }
        }

        private async Task RemoveShipmentDocument(RemovingEventArgs args)
        {
            var document = Shipping.SHIPPING_DOCUMENTS.SingleOrDefault(x => x.FILE_NAME == args.FilesData[0].Name);

            if (document == null) return;

            Shipping.SHIPPING_DOCUMENTS.Remove(document);

            await POAppService.UpdateShipping(Shipping.PO_LINE_ID, Shipping);
        }

        private async Task RemoveShipmentDocument(string filename)
        {
            var confirmed = await UIMessageService.Confirm("Are you sure to delete this file?");
            if (!confirmed) return;
            
            var document = Shipping.SHIPPING_DOCUMENTS.SingleOrDefault(x => x.FILE_NAME == filename);

            if (document == null) return;

            Shipping.SHIPPING_DOCUMENTS.Remove(document);

            await POAppService.UpdateShipping(Shipping.PO_LINE_ID, Shipping);
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
