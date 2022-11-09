using iwip.Import;
using iwip.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Alert = iwip.Models.Alert;

namespace iwip.Blazor.Pages.Import
{
    public partial class Index
    {
        [Inject] IImportAppService ImportAppService { get; set; }

        private bool isUploading = false;
        private List<Alert> Alerts { get; set; } = new();
        private string dropClass = string.Empty;
        private int maxAllowedFiles = 3;
        List<string> FileUrls = new List<string>();
        List<FileUploadProgress> filesQueue = new();
        List<FileChunkDto> Files = new();

        public Index() { }

        private async Task AddFilesToQueue(InputFileChangeEventArgs e)
        {
            dropClass = string.Empty;
            Alerts.Clear();

            if (e.FileCount > maxAllowedFiles)
            {
                Alerts.Add(new Alert(AlertType.warning, $"A maximum of {maxAllowedFiles} is allowed, you have selected {e.FileCount} files!"));
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
                        Alerts.Add(new Alert(AlertType.danger, $"Invalid file type {fileInfo.Extension} in file {fileInfo.Name}. Allowed file types {iWipConstants.AllowedExtensions}"));
                        return;
                    }
                    var progress = new FileUploadProgress(file, file.Name, file.Size, fileCount);
                    filesQueue.Add(progress);
                    fileCount++;
                }
            }

            await UploadFileQueue();

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
            // string newFileNameWithoutPath = $"{DateTime.Now.Ticks}-{nameOnly}{extension}";
            string newFileNameWithoutPath = $"{nameOnly}{extension}";

            bool firstChunk = true;
            using (var inStream = file.FileData.OpenReadStream(long.MaxValue))
            {
                for (int i = 0; i < numChunks; i++)
                {
                    var buffer = new byte[chunkSize];
                    await inStream.ReadAsync(buffer, 0, buffer.Length);

                    Files.Add(new FileChunkDto
                    {
                        Data = buffer,
                        FileName = newFileNameWithoutPath,
                        Offset = filesQueue[file.FileId].UploadedBytes,
                        FirstChunk = firstChunk
                    });

                    firstChunk = false;

                    // Update our progress data and UI
                    filesQueue[file.FileId].UploadedBytes += chunkSize;

                    await InvokeAsync(StateHasChanged);
                }

                if (remainder > 0)
                {
                    var buffer = new byte[remainder];
                    await inStream.ReadAsync(buffer, 0, buffer.Length);

                    Files.Add(new FileChunkDto
                    {
                        Data = buffer,
                        FileName = newFileNameWithoutPath,
                        Offset = filesQueue[file.FileId].UploadedBytes,
                        FirstChunk = firstChunk
                    });

                    // Update our progress data and UI
                    filesQueue[file.FileId].UploadedBytes += remainder;
                    //await ListFiles();
                    await InvokeAsync(StateHasChanged);
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
            public byte[] FileContent { get; set; }

        } //FileUploadProgress


        void HandleDragEnter()
        {
            dropClass = "dropzone-active";
        } //HandleDragEnter
        void HandleDragLeave()
        {
            dropClass = string.Empty;
        } //HandleDragLeave

        private async Task ImportData()
        {
            // get files
            try
            {
                foreach (var file in Files.GroupBy(x => x.FileName))
                {
                    var buffer = file.SelectMany(x => x.Data).ToArray();

                    string snam = Path.GetFileNameWithoutExtension(file.Key);
                    await ImportAppService.ImportAsync(snam, buffer);

                    Alerts.Add(new Alert(AlertType.success, "Import completed sucessfully."));
                }

            }
            catch (Exception ex)
            {
                Alerts.Add(new Alert(AlertType.danger, ex.Message));
            }
        }
    }
}