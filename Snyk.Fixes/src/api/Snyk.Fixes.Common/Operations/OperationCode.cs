namespace Snyk.Fixes.Common.Operations
{
    public enum OperationCode
    {
        // localhost operations
        Createlocalhost = 101,
        Updatelocalhost = 102,
        Deletelocalhost = 103,
        GetlocalhostById = 104,
        Searchlocalhost = 110,

        // Categories Operations
        CreateCategory = 201,
        UpdateCategory = 202,
        DeleteCategory = 203,

        // Items Operations
        CreatelocalhostItem = 301,
        UpdatelocalhostItem = 302,
        DeletelocalhostItem = 303
    }
}
