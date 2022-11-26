using iwip.PO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
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
    [MongoCollection("iw_po")]
    public IMongoCollection<PurchaseOrder> PurchaseOrders => Collection<PurchaseOrder>();
    [MongoCollection("iw_shipping")]
    public IMongoCollection<Shipping> Shipping => Collection<Shipping>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        BsonClassMap.RegisterClassMap<PurchaseOrder>(map =>
        {
            map.AutoMap();
            //map.SetIgnoreExtraElements(true);
            //map.MapProperty(x => x.TenantId).SetSerializer(new BsonBinaryDataSerializer());
        });

        BsonClassMap.RegisterClassMap<Shipping>(map =>
        {
            map.AutoMap();
        });

        //modelBuilder.Entity<TodoItems>(b =>
        //{
        //    b.CollectionName = "TodoItems";
        //});
    }

}
