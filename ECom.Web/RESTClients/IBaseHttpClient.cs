using System.Threading.Tasks;

namespace WebApp.RESTClients
{
    public interface IBaseHttpClient<TRequestEntity,TResponseEntity, TSearchParam,TSearchResult> 
        where TRequestEntity : class
        where TResponseEntity : class
        where TSearchParam : class
        where TSearchResult : class
    {
        Task<TSearchResult> GetSearchResult(TSearchParam searchParameter);

        Task<TResponseEntity> GetById(int id);

        Task<TResponseEntity> Create(TRequestEntity entity);
        Task<TResponseEntity> Update(TRequestEntity entity);
        Task Delete(int id);

    }
}
