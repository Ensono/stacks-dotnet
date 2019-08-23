namespace Amido.Stacks.Data.Documents.CosmosDB.Events
{
    public enum EventCode
    {
        GeneralException = 123456800,

        GetByIdRequested = 123456801,
        GetByIdCompleted = 123456802,
        GetByIdFailed = 123456803,

        SaveRequested = 123456811,
        SaveCompleted = 123456812,
        SaveFailed = 123456813,

        DeleteRequested = 123456821,
        DeleteCompleted = 123456822,
        DeleteFailed = 123456823,

        SearchRequested = 123456831,
        SearchCompleted = 123456832,
        SearchFailed = 123456833,

        SQLQueryRequested = 123456841,
        SQLQueryCompleted = 123456842,
        SQLQueryFailed = 123456843,
    }
}
