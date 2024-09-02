namespace xxENSONOxx.xxSTACKSxx.CQRS.Enums;

public enum EventCode
{
    // Menu operations
    MenuCreated = 101,
    MenuUpdated = 102,
    MenuDeleted = 103,

    // Categories Operations
    CategoryCreated = 201,
    CategoryUpdated = 202,
    CategoryDeleted = 203,

    // Items Operations
    MenuItemCreated = 301,
    MenuItemUpdated = 302,
    MenuItemDeleted = 303,

    // CosmosDB change feed operations
    EntityUpdated = 999,
    
    GeneralException = 123456899,

    PublishEventRequested = 123456801,
    PublishEventCompleted = 123456802,
    PublishEventFailed = 123456803
}
