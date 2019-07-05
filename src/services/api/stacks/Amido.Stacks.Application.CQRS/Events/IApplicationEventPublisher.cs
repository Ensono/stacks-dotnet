using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS.Events
{
    public interface IApplicationEventPublisher
    {
        Task PublishAsync(IApplicationEvent applicationEvent);
    }
}
