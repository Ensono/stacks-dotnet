using xxENSONOxx.xxSTACKSxx.Common.Operations;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Validators
{
    public interface IValidator<in T> where T: IOperationContext
    {
        void Validate(T message);
    }
}
