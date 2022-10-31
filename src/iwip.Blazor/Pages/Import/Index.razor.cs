using Blazorise;
using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using iwip.Import;
using iwip.PO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace iwip.Blazor.Pages.Import
{
    public partial class Index
    {
        [Inject] IImportAppService ImportAppService { get; set; }

        private bool isUploading = false;
        private string ErrorMessage = string.Empty;
        private string dropClass = string.Empty;
        private int maxAllowedFiles = 3;
        List<string> FileUrls = new List<string>();
        List<FileUploadProgress> filesQueue = new();

        private List<PurchaseOrderDto> PurchaseOrders { get; set; } = new List<PurchaseOrderDto>();

        public Index()
        {

        }

        //private async Task ImportAsync(FileUploadProgress file, bool isGeneric)
        //{
        //    string nameOnly = Path.GetFileNameWithoutExtension(file.FileName);
        //    var extension = Path.GetExtension(file.FileName);
        //    string text = System.IO.File.ReadAllText(file.FileName);

        //    var items = JsonConvert.DeserializeObject<ReadOnlyCollection<PurchaseOrderDto>>(text);

        //    await ImportAppService.ImportAsync(items);

        //    //StateHasChanged();
        //}

        private void AddFilesToQueue(InputFileChangeEventArgs e)
        {
            dropClass = string.Empty;
            ErrorMessage = string.Empty;

            if (e.FileCount > maxAllowedFiles)
            {
                ErrorMessage = $"A maximum of {maxAllowedFiles} is allowed, you have selected {e.FileCount} files!";
            }
            else
            {
                var files = e.GetMultipleFiles(maxAllowedFiles);
                var fileCount = filesQueue.Count;
                FileInfo fileInfo;

                foreach (var file in files)
                {
                    fileInfo = new FileInfo(file.Name);

                    if (!IsValidFileType(fileInfo))
                    {
                        ErrorMessage = $"Invalid file type {fileInfo.Extension} in file {fileInfo.Name}. Allowed file types {iWipConstants.AllowedExtensions}";
                        return;
                    }
                    var progress = new FileUploadProgress(file, file.Name, file.Size, fileCount);
                    filesQueue.Add(progress);
                    fileCount++;
                }
            }
        } //PlaceFilesInQue

        private bool IsValidFileType(FileInfo fileInfo)
        {
            var extension = fileInfo.Extension;
            return !string.IsNullOrEmpty(extension) && iWipConstants.AllowedExtensions.IndexOf(extension.ToLower()) >= 0;
        }

        private async Task UploadFileQueue()
        {
            isUploading = true;
            await InvokeAsync(StateHasChanged);

            foreach (var file in filesQueue.OrderByDescending(x => x.FileId))
            {
                if (!file.HasBeenUploaded)
                {
                    //* await ImportAsync(file, true);
                    await UploadChunks(file);
                    file.HasBeenUploaded = true;
                }
            }

            isUploading = false;
        } //UploadFileQueue


        private async Task UploadChunks(FileUploadProgress file)
        {
            var TotalBytes = file.Size;
            long chunkSize = 400000;
            long numChunks = TotalBytes / chunkSize;
            long remainder = TotalBytes % chunkSize;

            string nameOnly = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            string newFileNameWithoutPath = $"{DateTime.Now.Ticks}-{nameOnly}{extension}";

            bool firstChunk = true;
            using (var inStream = file.FileData.OpenReadStream(long.MaxValue))
            {
                for (int i = 0; i < numChunks; i++)
                {
                    var buffer = new byte[chunkSize];
                    await inStream.ReadAsync(buffer, 0, buffer.Length);

                    var chunk = new FileChunkDto
                    {
                        Data = buffer,
                        FileName = newFileNameWithoutPath,
                        Offset = filesQueue[file.FileId].UploadedBytes,
                        FirstChunk = firstChunk
                    };

                    firstChunk = false;

                    // Update our progress data and UI
                    filesQueue[file.FileId].UploadedBytes += chunkSize;
                    await InvokeAsync(StateHasChanged).ContinueWith(async task =>
                    {
                        // await ImportAppService.UploadFileChunk(chunk);
                        string text = System.Text.Encoding.Default.GetString(buffer); //System.IO.File.ReadAllText(filesQueue[file.FileId].FileName);
                        var items = JsonConvert.DeserializeObject<ReadOnlyCollection<PurchaseOrderDto>>(text);
                        await ImportAppService.ImportAsync(items);
                    });
                }

                if (remainder > 0)
                {
                    var buffer = new byte[remainder];
                    await inStream.ReadAsync(buffer, 0, buffer.Length);

                    var chunk = new FileChunkDto
                    {
                        Data = buffer,
                        FileName = newFileNameWithoutPath,
                        Offset = filesQueue[file.FileId].UploadedBytes,
                        FirstChunk = firstChunk
                    };

                    // Update our progress data and UI
                    filesQueue[file.FileId].UploadedBytes += remainder;
                    //await ListFiles();
                    await InvokeAsync(StateHasChanged).ContinueWith(async task =>
                    {
                        // await ImportAppService.UploadFileChunk(chunk);
                        string text = System.Text.Encoding.Default.GetString(buffer); //System.IO.File.ReadAllText(filesQueue[file.FileId].FileName);
                        var items = JsonConvert.DeserializeObject<ReadOnlyCollection<PurchaseOrderDto>>(text);
                        await ImportAppService.ImportAsync(items);
                    });
                }
            }
        } //UploadChunks


        private void RemoveFromQueue(int fileId)
        {
            var itemToRemove = filesQueue.SingleOrDefault(x => x.FileId == fileId);
            if (itemToRemove != null)
                filesQueue.Remove(itemToRemove);
        } //RemoveFromQueue


        private void ClearFileQueue()
        {
            filesQueue.Clear();
        } //ClearFileQueue        


        record FileUploadProgress(IBrowserFile File, string FileName, long Size, int FileId)
        {
            public IBrowserFile FileData { get; set; } = File;
            public int FileId { get; set; } = FileId;
            public long UploadedBytes { get; set; }
            public double UploadedPercentage => (double)UploadedBytes / (double)Size * 100d;
            public bool HasBeenUploaded { get; set; } = false;
        } //FileUploadProgress


        void HandleDragEnter()
        {
            dropClass = "dropzone-active";
        } //HandleDragEnter
        void HandleDragLeave()
        {
            dropClass = string.Empty;
        } //HandleDragLeave


        /*
        protected override async Task OnInitializedAsync()
        {
            await ListFiles();
        }
        private async Task ListFiles()
        {
            FileUrls = await ImportAppService.GetFileNames();
            await InvokeAsync(StateHasChanged);
        }
        */


    }

}