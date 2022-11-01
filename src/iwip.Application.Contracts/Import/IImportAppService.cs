using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace iwip.Import
{
    public interface IImportAppService : IApplicationService
    {
        Task ImportAsync(string collectionName, byte[] content);
    }
}
