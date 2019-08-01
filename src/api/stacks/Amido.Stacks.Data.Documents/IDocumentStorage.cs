using System.Threading.Tasks;

namespace Amido.Stacks.Data.Documents
{
    public interface IDocumentStorage<TEntity, in TEntityIdentityType> where TEntity : class
    {
        Task<OperationResult<TEntity>> SaveAsync(TEntityIdentityType identifier, string partitionKey, TEntity document, string eTag);
        Task<OperationResult<TEntity>> GetByIdAsync(TEntityIdentityType identifier, string partitionKey);
        Task<OperationResult> DeleteAsync(TEntityIdentityType identifier, string partitionKey);
    }
}
