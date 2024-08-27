using System.Collections.Generic;

namespace xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB
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
