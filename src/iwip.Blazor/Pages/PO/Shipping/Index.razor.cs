using iwip.Helpers.Extensions;
using iwip.PO;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Users;
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

        private async Task OnFileChangeUpload(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                try
                {
                    var path = @"path" + file.FileInfo.Name;
                    string base64;

                    FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                    await file.Stream.CopyToAsync(filestream);
                    
                    using (var reader = new System.IO.StreamReader(file.Stream))
                    {
                        byte[] buffer = new byte[file.Stream.Length];
                        var inputStream = new MemoryStream(buffer);
                        base64 = Convert.ToBase64String(inputStream.ToArray());
                    }
            
                    Shipping.SHIPPING_DOCUMENTS.Add(new ShippingDocumentDto
                    {
                        FILE_NAME = file.FileInfo.Name,
                        MIME_TYPE = MimeTypes.GetMimeType(path),
                        CONTENT = base64,
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = CurrentUser.Id
                    });

                    await POAppService.InsertShippingDocument(Shipping.PO_LINE_ID, Shipping);

                    //Shipping.SHIPPING_DOCUMENTS.Add(new ShippingDocumentDto
                    //{
                    //    NAME = file.FileInfo.Name,
                    //    CREATE_DATE = DateTime.Now,
                    //    FILE_CONTENT = base64,
                    //    MIME_TYPE = MimeTypes.GetMimeType(path),
                    //    CREATED_BY = CurrentUser.Id

                    //});
                    //filestream.Close();
                    //file.Stream.Close();
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }

            // var updateDto = Mapper.Map<PurchaseOrderDto, CreateUpdatePODto>(PurchaseOrder);

            // POAppService.UpdateAsync(PurchaseOrder);
        }


        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }

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
