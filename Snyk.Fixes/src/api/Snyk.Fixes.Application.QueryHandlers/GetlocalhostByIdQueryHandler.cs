using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.Queries.GetlocalhostById;

namespace Snyk.Fixes.Application.QueryHandlers
{
    public class GetlocalhostByIdQueryHandler : IQueryHandler<GetlocalhostById, localhost>
    {
        private readonly IlocalhostRepository repository;

        public GetlocalhostByIdQueryHandler(IlocalhostRepository repository)
        {
            this.repository = repository;
        }

        public async Task<localhost> ExecuteAsync(GetlocalhostById criteria)
        {
            var localhost = await repository.GetByIdAsync(criteria.Id);

            if (localhost == null)
                return null;

            //You might prefer using AutoMapper in here
            var result = new localhost()
            {
                Id = localhost.Id,
                TenantId = localhost.TenantId,
                Name = localhost.Name,
                Description = localhost.Description,
                Enabled = localhost.Enabled,
                Categories = localhost.Categories?.Select(c =>
                    new Category()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Items = c.Items?.Select(i =>
                            new localhostItem()
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Description = i.Description,
                                Price = i.Price,
                                Available = i.Available
                            }
                        ).ToList()
                    }).ToList()
            };

            return result;
        }
    }
}
