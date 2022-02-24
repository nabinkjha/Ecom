using System.Threading.Tasks;

namespace WebApp.RESTClients
{
    public interface IBaseHttpClient<TEntity, TParam,TResult> 
        where TEntity : class
         where TParam : class
         where TResult : class
    {
        Task<TResult> GetSearchResult(TParam searchParameter);

        Task<TEntity> GetProductById(int id);

        Task<TEntity> CreateProduct(TEntity entity);
        Task<TEntity> UpdateProduct(TEntity entity);
        Task<TEntity> DeleteProduct(int id);

    }
}
