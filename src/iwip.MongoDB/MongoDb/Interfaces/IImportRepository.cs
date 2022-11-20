using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace iwip.MongoDb.Interfaces
{
    public interface IImportRepository : IRepository
    {
        Task ImportManyAsync(string collectionName, string json);
    }
}
