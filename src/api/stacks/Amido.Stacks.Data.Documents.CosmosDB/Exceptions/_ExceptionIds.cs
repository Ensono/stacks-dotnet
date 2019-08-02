namespace Amido.Stacks.Data.Documents.CosmosDB.Exceptions
{
    public enum ExceptionIds
    {
        ResourceNotFound = 99900100,
        DocumentHasChanged = 99900101,
        InvalidSearchParameter = 99900102,
        DocumentUpsertFailed = 99900103,
        DocumentRetrievalFailed = 99900104,
        DocumentDeletionFailed = 99900105,
        PartitionKeyRequired = 99900106
    }
}
