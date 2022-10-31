using iwip.PO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace iwip.MongoDB;

[ConnectionStringName("Default")]
public class iwipMongoDbContext : AbpMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    /*[MongoCollection("s_po")]*/
    public IMongoCollection<PurchaseOrder> PurchaseOrders => Collection<PurchaseOrder>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        /*BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));*/
        //BsonClassMap.RegisterClassMap<TodoItem>(map =>
        //{
        //    map.AutoMap();
        //    map.MapProperty(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
        //});
        modelBuilder.Entity<PurchaseOrder>(b =>
        {
            b.CollectionName = "TodoItems";
        });



    }
}
