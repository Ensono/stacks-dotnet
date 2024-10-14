namespace xxENSONOxx.xxSTACKSxx.Abstractions.Queries;

public interface IQueryHandler<in TQueryCriteria, TResult> where TQueryCriteria : class, IQueryCriteria
{
    Task<TResult> ExecuteAsync(TQueryCriteria criteria);
}
