using iwip.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iwip.MongoDB.iwipMongoDbContext;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using iwip.PO;
using System.Collections;
using iwip.MongoDb.Interfaces;

namespace iwip.MongoDb
{
    public class PORepository : MongoDbRepository<iwipMongoDbContext, BaseEntity, Guid>, IPORepository, ITransientDependency
    {
        IMongoDbContextProvider<iwipMongoDbContext> _context;

        public PORepository(IServiceProvider serviceProvider, IMongoDbContextProvider<iwipMongoDbContext> context) : base(context)
        {
            _context = context;
        }

        public async Task<ShippingDocument> InsertShippingDocument(Guid id, int lineId, ShippingDocument attachment)
        {
            var dbContext = await _context.GetDbContextAsync();
            var client = dbContext.Client;
            var database = client.GetDatabase("iwip");

            var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("_id", id),
                    Builders<BsonDocument>.Filter.Eq("PO_LINES.PO_LINE_ID", lineId));

            var collection = database.GetCollection<BsonDocument>("iw_shipping");

            var update = Builders<BsonDocument>.Update.Push("SHIPPING_DOCUMENTS.$.SHIPPING", attachment);

            await collection.FindOneAndUpdateAsync(filter, update);
            return attachment;

        }
    }
}
