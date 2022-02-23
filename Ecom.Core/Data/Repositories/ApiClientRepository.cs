using ECom.Contracts.Data.Repositories;
using ECom.Core.Entities;
using System.Linq;

namespace ECom.Core.Data.Repositories
{
    public class ApiClientRepository : Repository<ApiClient>, IApiClientRepository
    {
        public ApiClientRepository(DatabaseContext context) : base(context)
        {
        }

        public bool CheckValidApiKey(string apiKey)
        {
            var result = Where(x => x.Key == apiKey && !x.IsBlocked).ToList();
            return result.Count > 0;
        }
    }
}