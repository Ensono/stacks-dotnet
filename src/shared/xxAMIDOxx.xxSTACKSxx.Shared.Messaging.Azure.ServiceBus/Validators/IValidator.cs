using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Validators
{
    public interface IValidator<in T> where T: IOperationContext
    {
        void Validate(T message);
    }
}