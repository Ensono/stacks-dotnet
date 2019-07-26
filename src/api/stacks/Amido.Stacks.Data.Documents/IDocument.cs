namespace Amido.Stacks.Data.Documents
{
    public interface IDocument<out TDocumentIdentifier> //, out TDocumentPartitioner>
    {
        TDocumentIdentifier Identity { get; }

        //TDocumentPartitioner Partition { get; }

        string ETag { get; }
    }
}
