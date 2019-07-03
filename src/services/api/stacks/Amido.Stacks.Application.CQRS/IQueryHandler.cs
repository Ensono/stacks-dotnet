using System;
using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS
{
    public interface IQueryHandler<in TQueryCriteria, TResult> where TQueryCriteria : class
    {
        Task<TResult> Execute(TQueryCriteria criteria);
    }
}
