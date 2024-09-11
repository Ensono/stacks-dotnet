namespace xxENSONOxx.xxSTACKSxx.Common.Events;

public enum EventCode
{
    // Menu operations
    MenuCreated = 101,
    MenuUpdated = 102,
    MenuDeleted = 103,

    //GetMenu = 104,
    //SearchMenu = 110,

    // Categories Operations
    CategoryCreated = 201,
    CategoryUpdated = 202,
    CategoryDeleted = 203,

    // Items Operations
    MenuItemCreated = 301,
    MenuItemUpdated = 302,
    MenuItemDeleted = 303,

    CosmosDbChangeFeedEvent = 999,
#if (DynamoDb)
     // DynamoDB Operations
    DynamoDbGeneralException = 123456899,

    DynamoDbGetByIdRequested = 123456801,
    DynamoDbGetByIdCompleted = 123456802,
    DynamoDbGetByIdFailed = 123456803,

    DynamoDbSaveRequested = 123456811,
    DynamoDbSaveCompleted = 123456812,
    DynamoDbSaveFailed = 123456813,

    DynamoDbDeleteRequested = 123456821,
    DynamoDbDeleteCompleted = 123456822,
    DynamoDbDeleteFailed = 123456823,

    DynamoDbScanAsyncRequested = 123456831,
    DynamoDbScanAsyncCompleted = 123456832,
    DynamoDbScanAsyncFailed = 123456833,

    DynamoDbQueryAsyncRequested = 123456841,
    DynamoDbQueryAsyncCompleted = 123456842,
    DynamoDbQueryAsyncFailed = 123456843,
#endif
#if (CosmosDb)
    // CosmosDB Operations
    CosmosDbInitializing = 123456800,
    CosmosDbGeneralException = 123456899,

    CosmosDbGetByIdRequested = 123456801,
    CosmosDbGetByIdCompleted = 123456802,
    CosmosDbGetByIdFailed = 123456803,

    CosmosDbSaveRequested = 123456811,
    CosmosDbSaveCompleted = 123456812,
    CosmosDbSaveFailed = 123456813,

    CosmosDbDeleteRequested = 123456821,
    CosmosDbDeleteCompleted = 123456822,
    CosmosDbDeleteFailed = 123456823,

    CosmosDbSearchRequested = 123456831,
    CosmosDbSearchCompleted = 123456832,
    CosmosDbSearchFailed = 123456833,

    CosmosDbSQLQueryRequested = 123456841,
    CosmosDbSQLQueryCompleted = 123456842,
    CosmosDbSQLQueryFailed = 123456843,
#endif
}
