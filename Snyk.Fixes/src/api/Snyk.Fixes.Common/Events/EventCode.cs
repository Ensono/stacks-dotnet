namespace Snyk.Fixes.Common.Events
{
    public enum EventCode
    {
        // localhost operations
        localhostCreated = 101,
        localhostUpdated = 102,
        localhostDeleted = 103,

        //Getlocalhost = 104,
        //Searchlocalhost = 110,

        // Categories Operations
        CategoryCreated = 201,
        CategoryUpdated = 202,
        CategoryDeleted = 203,

        // Items Operations
        localhostItemCreated = 301,
        localhostItemUpdated = 302,
        localhostItemDeleted = 303
    }
}
