using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Queries
{
    public interface IQueryHandler<in TQueryCriteria, TResult> where TQueryCriteria : class, IQueryCriteria
    {
        Task<TResult> ExecuteAsync(TQueryCriteria criteria);
    }
}
