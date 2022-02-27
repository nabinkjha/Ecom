using System.Threading.Tasks;

namespace WebApp.RESTClients
{
    public interface IBaseHttpClient<TEntity, TParam,TResult> 
        where TEntity : class
         where TParam : class
         where TResult : class
    {
        Task<TResult> GetSearchResult(TParam searchParameter);

        Task<TEntity> GetById(int id);

        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Delete(int id);

    }
}
