using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Validators
{
    public interface IValidator<in T> where T: IOperationContext
    {
        void Validate(T message);
    }
}
