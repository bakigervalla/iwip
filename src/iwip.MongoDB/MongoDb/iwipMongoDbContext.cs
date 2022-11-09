using iwip.PO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.Identity;
using Volo.Abp.MongoDB;

namespace iwip.MongoDB;

[ConnectionStringName("Default")]
public class iwipMongoDbContext : AbpMongoDbContext
{
    [MongoCollection("PurchaseOrders")]
    public IMongoCollection<PurchaseOrder> PurchaseOrders => Collection<PurchaseOrder>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //BsonClassMap.RegisterClassMap<PurchaseOrder>(map =>
        //{
        //    map.AutoMap();
        //    map.SetIgnoreExtraElements(true);
        //    // map.MapProperty(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
        //});

        //modelBuilder.Entity<TodoItems>(b =>
        //{
        //    b.CollectionName = "TodoItems";
        //});
    }

    public class ImportRepository : MongoDbRepository<iwipMongoDbContext, BaseEntity, Guid>, IImportRepository, ITransientDependency
    {

        IMongoDbContextProvider<iwipMongoDbContext> _context;
        private readonly IServiceProvider _serviceProvider;

        public ImportRepository(IServiceProvider serviceProvider, IMongoDbContextProvider<iwipMongoDbContext> context) : base(context)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task ImportManyAsync(string collectionName, string json)
        {
            var dbContext = await _context.GetDbContextAsync();
            var client = dbContext.Client;
            var database = client.GetDatabase("iwip");

            // Guid.NewGuid().ToString() = "CSUUID("0ad3254f - d8a8 - 4951 - 82e4 - 7e36b7e6546c")"
            // $"BinData(3, '{Guid.NewGuid()}')" = $"BinData(3, '0ad3254f - d8a8 - 4951 - 82e4 - 7e36b7e6546c')"

            // Insert Id as UUID
            //var jObj = JArray.Parse(json);
            //foreach (var item in jObj)
            //{
            //    foreach (var val in item.Values())
            //    {
            //        var str = DateTime.TryParse(val.ToString(), out DateTime outDate);
            //        if (str == true)
            //            item[((Newtonsoft.Json.Linq.JProperty)val.Parent).Name] = outDate;
            //            // ((Newtonsoft.Json.Linq.JValue)val).Value = outDate;
            //    }
            //    //    item["_id"] = Guid.NewGuid(); // Guid.NewGuid().ToString(); // $"BinData(3, '{Guid.NewGuid()}')";
            //}
            //var newjson = jObj.ToString(Newtonsoft.Json.Formatting.Indented);

            var documents = BsonSerializer.Deserialize<List<BsonDocument>>(json);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            int i = 0;
            foreach (var document in documents)
            {
                document.Add("_id", new BsonBinaryData(Guid.NewGuid()));
                foreach (BsonElement el in document.Elements.ToList())
                {
                    if (el.Value.IsBsonArray)
                        ParseArrayElement(el.Value.AsBsonArray);
                    if (el.Value.IsBsonDocument)
                        ParseDocumentElement(el.Value.AsBsonDocument);
                    else
                    {
                        var str = DateTime.TryParse(el.Value.ToString(), out DateTime outDate);
                        if (str)
                            document.SetElement(i, new BsonElement(el.Name, outDate));
                    }
                    i++;
                }
                i = 0;
            }

            await collection.InsertManyAsync(documents);
        }

        private BsonArray ParseArrayElement(BsonArray documents)
        {
            int i = 0;
            foreach (var document in documents)
            {
                foreach (var el in ((BsonDocument)document).Elements.ToList())
                {
                    if (el.Value.IsBsonArray)
                        ParseArrayElement(el.Value.AsBsonArray);
                    if (el.Value.IsBsonDocument)
                        ParseDocumentElement(el.Value.AsBsonDocument);

                    var str = DateTime.TryParse(el.Value.ToString(), out DateTime outDate);
                    if (str)
                        ((BsonDocument)document).SetElement(i, new BsonElement(el.Name, outDate));
                    i++;
                }
                i = 0;
            }
            return documents;
        }

        private BsonDocument ParseDocumentElement(BsonDocument document)
        {
            int i = 0;
            foreach (var el in document.Elements.ToList())
            {
                if (el.Value.IsBsonDocument)
                    ParseDocumentElement(el.Value.AsBsonDocument);

                var str = DateTime.TryParse(el.Value.ToString(), out DateTime outDate);
                if (str)
                    ((BsonDocument)document).SetElement(i, new BsonElement(el.Name, outDate));
                i++;
            }
            return document;
        }

        // Obsolete: Another method
        public async Task ObsoleteImportManyAsync(string collectionName, string json)
        {
            //try
            //{
            //    // BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            //    // BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            //    // BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

            //    var dbContext = _serviceProvider.GetServices<IAbpMongoDbContext>().FirstOrDefault();
            //    var connectionStringResolver = _serviceProvider.GetRequiredService<IConnectionStringResolver>();


            //    var connectionString = await connectionStringResolver.ResolveAsync(ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType()));
            //    var mongoUrl = new MongoUrl(connectionString);
            //    var databaseName = mongoUrl.DatabaseName;
            //    var client = new MongoClient(mongoUrl);

            //    var database = client.GetDatabase(databaseName);

            //    var documents = BsonSerializer.Deserialize<IEnumerable<BsonDocument>>(json);
            //    var collection = database.GetCollection<BsonDocument>(collectionName);

            //    await collection.InsertManyAsync(documents);

            //}

            //catch (Exception ex)

            //{
            //    var mess = ex.Message;
            //}
        }
    }

    public interface IImportRepository : IRepository
    {
        Task ImportManyAsync(string collectionName, string json);
    }

}
