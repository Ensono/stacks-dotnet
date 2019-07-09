﻿using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS.Queries
{
    public interface IQueryHandler<in TQueryCriteria, TResult> where TQueryCriteria : class
    {
        Task<TResult> ExecuteAsync(TQueryCriteria criteria);
    }
}
