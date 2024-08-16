using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents
{
    public class OperationResult<TContent>(bool isSuccessful, TContent content, Dictionary<string, string> attributes)
        : OperationResult(isSuccessful, attributes)
    {
        public TContent Content { get; } = content;
    }

    public class OperationResult(bool isSuccessful, Dictionary<string, string> attributes)
    {
        public bool IsSuccessful { get; } = isSuccessful;

        public Dictionary<string, string> Attributes { get; } = attributes;
    }
}
