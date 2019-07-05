using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS
{
    public interface IApplicationEventPublisher
    {
        Task PublishAsync(IApplicationEvent applicationEvent);
    }
}
