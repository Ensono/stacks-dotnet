using System.Threading.Tasks;

namespace Amido.Stacks.Data.Documents.Abstractions
{
    public interface IDocumentStorage<TEntity> where TEntity : class
    {
        Task<OperationResult<TEntity>> SaveAsync(string identifier, string partitionKey, TEntity document, string eTag);
        Task<OperationResult<TEntity>> GetByIdAsync(string identifier, string partitionKey);
        Task<OperationResult> DeleteAsync(string identifier, string partitionKey);
    }
}
