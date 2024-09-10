using System.Collections.Generic;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions
{
    public class CosmosOperationResult<T>(
        bool isSuccessful,
        T content,
        Dictionary<string, string> attributes,
        double requestCharge)
        : OperationResult<T>(isSuccessful, content, attributes)
    {
        public double RequestCharge { get; } = requestCharge;
    }
}
