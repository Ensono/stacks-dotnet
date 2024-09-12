#if CosmosDb
namespace xxENSONOxx.xxSTACKSxx.Common.Exceptions.CosmosDb;

public enum ExceptionIds
{
    ResourceNotFound = 99900100,
    DocumentHasChanged = 99900101,
    InvalidSearchParameter = 99900102,
    DocumentUpsertFailed = 99900103,
    DocumentRetrievalFailed = 99900104,
    DocumentDeletionFailed = 99900105,
    PartitionKeyRequired = 99900106,
    NullParameter = 99900107,

    CosmosDBRequestCapacityExceeded = 99900429
}
#endif
