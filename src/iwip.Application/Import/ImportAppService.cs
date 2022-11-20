using iwip.MongoDb.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace iwip.Import
{
    public class ImportAppService : ApplicationService, IImportAppService
    {
        private readonly IImportRepository _importRepository;

        public ImportAppService(IImportRepository importRepository)
        {
            _importRepository = importRepository;
        }

        public async Task ImportAsync(string collectionName, byte[] content)
        {

            string text = Encoding.ASCII.GetString(content);

            await _importRepository.ImportManyAsync(collectionName, text);
        }

        private Guid IntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

    }
}
