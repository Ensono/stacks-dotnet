using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.Abstractions
{
    public interface IDocumentSearch<TEntity>
    {
        Task<OperationResult<IEnumerable<TEntity>>> Search(
            Expression<Func<TEntity, bool>> searchPredicate,
            string partitionKey = null,
            int pageSize = 20,
            int pageNumber = 1
            );

        Task<OperationResult<IEnumerable<TResult>>> Search<TResult>(
            Expression<Func<TResult, bool>> searchPredicate,
            string partitionKey = null,
            int pageSize = 20,
            int pageNumber = 1
            );

        Task<OperationResult<IEnumerable<TResult>>> Search<TResult, TOrderKey>(
            Expression<Func<TResult, bool>> searchPredicate,
            Expression<Func<TResult, TOrderKey>> orderPredicate = null,
            bool isAscendingOrder = true,
            string partitionKey = null,
            int pageSize = 20,
            int pageNumber = 1
        );

        Task<OperationResult<IEnumerable<TResult>>> RunSQLQueryAsync<TResult>(
            string sqlQuery,
            Dictionary<string, object> parameters = null,
            string partitionKey = null,
            int? MaxItemCount = null,
            string continuationToken = null
        );
    }
}
