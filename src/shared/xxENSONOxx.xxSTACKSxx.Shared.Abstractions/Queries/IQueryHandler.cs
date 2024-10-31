namespace xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Queries;

public interface IQueryHandler<in TQueryCriteria, TResult> where TQueryCriteria : class, IQueryCriteria
{
    Task<TResult> ExecuteAsync(TQueryCriteria criteria);
}
