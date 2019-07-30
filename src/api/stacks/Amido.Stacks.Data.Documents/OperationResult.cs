using System.Collections.Generic;

namespace Amido.Stacks.Data.Documents
{
    public class OperationResult<TContent> : OperationResult
    {
        public TContent Content { get; }

        public OperationResult(bool isSuccessful, TContent content, Dictionary<string, string> attributes) : base(isSuccessful, attributes)
        {
            Content = content;
        }
    }

    public class OperationResult
    {
        public bool IsSuccessful { get; }


        public Dictionary<string, string> Attributes { get; }

        public OperationResult(bool isSuccessful, Dictionary<string, string> attributes)
        {
            IsSuccessful = isSuccessful;
            Attributes = attributes;
        }
    }
}
