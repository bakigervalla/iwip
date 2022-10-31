using AutoMapper.Internal.Mappers;
using iwip.PO;
using Microsoft.Extensions.DependencyInjection;
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
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace iwip.Import
{
    public class ImportAppService : ApplicationService, IImportAppService
    {
        private readonly IRepository<PurchaseOrder, Guid> _importRepository;
        private readonly IDataFilter _dataFilter;

        public ImportAppService(IRepository<PurchaseOrder, Guid> importRepository)
        {
            _importRepository = importRepository;
        }

        HttpClient _http;
        public ImportAppService(HttpClient http)
        {
            _http = http;
        } //ImportAppService


        public async Task<bool> UploadFileChunk(FileChunkDto fileChunkDto)
        {
            try
            {
                var result = await _http.PostAsJsonAsync("api/Files/UploadFileChunk", fileChunkDto);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                return Convert.ToBoolean(responseBody);
            }
            catch (Exception ex)
            {
                return false;
            }
        } //UploadFileChunk


        public async Task<List<string>> GetFileNames()
        {
            try
            {
                var response = await _http.GetAsync("api/Files/GetFiles");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<string>>(responseBody);
            }
            catch (Exception ex)
            {
                return null;
            }
        } //GetFileNames

        //NOTE: Multitenant
        public async Task ImportAsync(ReadOnlyCollection<PurchaseOrderDto> purchaseOrders)
        {
            var items = ObjectMapper.Map<ReadOnlyCollection<PurchaseOrderDto>, ReadOnlyCollection<PurchaseOrder>>(purchaseOrders);
            var tenantId = items.FirstOrDefault().MANUFACTURER;

            //using (_dataFilter.Disable<IMultiTenant>())
            //{
            //    return await _productRepository.GetCountAsync();
            //}

            using (CurrentTenant.Change(IntToGuid(tenantId)))
            {
                await _importRepository.InsertManyAsync(items);
            }
            
        }

        private Guid IntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public Task ImportJsonAsync(string json)
        {
            return null;
                //1.
            /*                   var connectionString = "<Connection String>"; //example: ... = "mongodb://localhost";

                        var client = new MongoClient(connectionString);
                        var database = client.GetDatabase("<Database Name>");  //example: ...GetDatabase("test"); 

                        string text = System.IO.File.ReadAllText(@"<File Directory with FileName.json>"); //example: ...ReadAllText(@"MyMovies.json");

                        var document = BsonSerializer.Deserialize<BsonDocument>(text);
                        var collection = database.GetCollection<BsonDocument>("<Collection Name>"); // example: ...<BsonDocument>("Movies");
                        await collection.InsertOneAsync(document);*/

            //2.
            //    string inputFileName; // initialize to the input file
            //IMongoCollection<BsonDocument> collection; // initialize to the collection to write to.

            //using (var streamReader = new StreamReader(inputFileName))
            //{
            //    string line;
            //    while ((line = await streamReader.ReadLineAsync()) != null)
            //    {
            //        using (var jsonReader = new JsonReader(line))
            //        {
            //            var context = BsonDeserializationContext.CreateRoot(jsonReader);
            //            var document = collection.DocumentSerializer.Deserialize(context);
            //            await collection.InsertOneAsync(document);
            //        }
            //    }
            //}

            //3.
            //var connectionString = "mongodb://localhost";

            //           var client = new MongoClient(connectionString);
            //           var database = client.GetDatabase("test");

            //           string text = System.IO.File.ReadAllText(@"records.JSON");

            //           var document = BsonSerializer.Deserialize<BsonDocument>(text);
            //           var collection = database.GetCollection<BsonDocument>("test_collection");
            //           await collection.InsertOneAsync(document);
        }


    }
}
