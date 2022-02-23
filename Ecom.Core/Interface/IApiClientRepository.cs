using ECom.Core.Entities;

namespace ECom.Contracts.Data.Repositories
{
    public interface IApiClientRepository : IRepository<ApiClient>
    {
        bool CheckValidApiKey(string apiKey);
    }
}