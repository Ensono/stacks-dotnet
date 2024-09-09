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

    // DynamoDB Operations
    GeneralException = 123456899,

	GetByIdRequested = 123456801,
	GetByIdCompleted = 123456802,
	GetByIdFailed = 123456803,

	SaveRequested = 123456811,
	SaveCompleted = 123456812,
	SaveFailed = 123456813,

	DeleteRequested = 123456821,
	DeleteCompleted = 123456822,
	DeleteFailed = 123456823,

	ScanAsyncRequested = 123456831,
	ScanAsyncCompleted = 123456832,
	ScanAsyncFailed = 123456833,

	QueryAsyncRequested = 123456841,
	QueryAsyncCompleted = 123456842,
	QueryAsyncFailed = 123456843,
}
