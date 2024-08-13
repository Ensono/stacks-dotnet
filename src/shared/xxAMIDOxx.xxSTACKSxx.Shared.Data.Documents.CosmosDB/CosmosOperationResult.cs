using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB
{
    public class CosmosOperationResult<T> : OperationResult<T>
    {
        public double RequestCharge { get; }

        public CosmosOperationResult(bool isSuccessful, T content, Dictionary<string, string> attributes, double requestCharge) : base(isSuccessful, content, attributes)
        {
            RequestCharge = requestCharge;
        }
    }
}
