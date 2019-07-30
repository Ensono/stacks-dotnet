using System.Threading.Tasks;

namespace Amido.Stacks.Data.Documents
{
    public interface IDocumentRepository<TEntity, in TEntityIdentityType> //where TEntity : class, IEntity<TEntityIdentifier>
    {
        Task<OperationResult<TEntity>> SaveAsync(TEntityIdentityType identifier, string partitionKey, TEntity document, string eTag);
        Task<OperationResult<TEntity>> GetByIdAsync(TEntityIdentityType identifier, string partitionKey);
        Task<OperationResult> DeleteAsync(TEntityIdentityType identifier, string partitionKey);

        //TODO: Decide about partition scan and cross partition scan
        //Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> searchPredicate, int queryLimit = -1);
        //Task<IEnumerable<TEntity>> SearchAsync<TKey>(Expression<Func<TEntity, bool>> searchPredicate, Expression<Func<TEntity, TKey>> orderPredicate, bool isAscending = true, int queryLimit = -1);

        //TODO: USE 'SELECT VALUE COUNT(1) FROM c' for count function instead of 
        //int Count();
        //int Count(Expression<Func<TEntity, bool>> searchPredicate);

    }
}
