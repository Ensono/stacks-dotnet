using System.Collections.Generic;

namespace Amido.Stacks.Data.Documents.CosmosDB
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
