#if (CosmosDb)
using System;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Common.Events;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Logging
{
    /// <summary>
    /// Contains log definitions for CosmosDB component
    /// LoggerMessage.Define() creates a unique template for each log type
    /// The log template reduces the number of allocations and write logs faster to destination
    /// </summary>
    public static class CosmosDbLogDefinition
    {
        /// Failures with exceptions should be logged to respective failures(i.e: getByIdFailed) and then to logException in order to show them as separate entries in the logs(trace + exception
        private static readonly Action<ILogger, string, Exception> logException =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId((int)EventCode.CosmosDbGeneralException, nameof(EventCode.CosmosDbGeneralException)),
                "CosmosDB Exception: {Message}"
            );

        private static readonly Action<ILogger, string, string, string, Exception> initializing =
            LoggerMessage.Define<string, string, string>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbInitializing, nameof(EventCode.CosmosDbInitializing)),
                "CosmosDB: Initializing database connection to collection '{CollectionName}' on database '{DatabaseName}' from cosmos database account '{DatabaseAccountUri}'."
            );

        //GETById
        private static readonly Action<ILogger, string, string, string, Exception> getByIdRequested =
            LoggerMessage.Define<string, string, string>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbGetByIdRequested, nameof(EventCode.CosmosDbGetByIdRequested)),
                "CosmosDB: GetById requested for document {CollectionName}(Partition:{Partition}, id:{ResourceId})"
            );

        private static readonly Action<ILogger, string, string, string, double, Exception> getByIdCompleted =
            LoggerMessage.Define<string, string, string, double>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbGetByIdCompleted, nameof(EventCode.CosmosDbGetByIdCompleted)),
                "CosmosDB: GetById completed for document {CollectionName}(Partition:{Partition}, id:{ResourceId}). Request Charge: {RequestCharge}."
            );

        private static readonly Action<ILogger, string, string, string, string, Exception> getByIdFailed =
            LoggerMessage.Define<string, string, string, string>(
                LogLevel.Warning,
                new EventId((int)EventCode.CosmosDbGetByIdFailed, nameof(EventCode.CosmosDbGetByIdFailed)),
                "CosmosDB: GetById failed for document {CollectionName}(Partition:{Partition}, id:{ResourceId}). Reason: {Reason}"
            );

        //SAVE
        private static readonly Action<ILogger, string, string, string, Exception> saveRequested =
            LoggerMessage.Define<string, string, string>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbSaveRequested, nameof(EventCode.CosmosDbSaveRequested)),
                "CosmosDB: Save requested for document {CollectionName}(Partition:{Partition}, id:{ResourceId})"
            );

        private static readonly Action<ILogger, string, string, string, double, Exception> saveCompleted =
            LoggerMessage.Define<string, string, string, double>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbSaveCompleted, nameof(EventCode.CosmosDbSaveCompleted)),
                "CosmosDB: Save completed for document {CollectionName}(Partition:{Partition}, id:{ResourceId}). Request Charge: {RequestCharge}."
            );

        private static readonly Action<ILogger, string, string, string, string, Exception> saveFailed =
            LoggerMessage.Define<string, string, string, string>(
                LogLevel.Warning,
                new EventId((int)EventCode.CosmosDbSaveFailed, nameof(EventCode.CosmosDbSaveFailed)),
                "CosmosDB: Save failed for document {CollectionName}(Partition:{Partition}, id:{ResourceId}). Reason: {Reason}"
            );

        //DELETE
        private static readonly Action<ILogger, string, string, string, Exception> deleteRequested =
            LoggerMessage.Define<string, string, string>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbDeleteRequested, nameof(EventCode.CosmosDbDeleteRequested)),
                "CosmosDB: Delete requested for document {CollectionName}(Partition:{Partition}, id:{ResourceId})"
            );

        private static readonly Action<ILogger, string, string, string, double, Exception> deleteCompleted =
            LoggerMessage.Define<string, string, string, double>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbDeleteCompleted, nameof(EventCode.CosmosDbDeleteCompleted)),
                "CosmosDB: Delete completed for document {CollectionName}(Partition:{Partition}, id:{ResourceId}). Request Charge: {RequestCharge}."
            );

        private static readonly Action<ILogger, string, string, string, string, Exception> deleteFailed =
            LoggerMessage.Define<string, string, string, string>(
                LogLevel.Warning,
                new EventId((int)EventCode.CosmosDbDeleteFailed, nameof(EventCode.CosmosDbDeleteFailed)),
                "CosmosDB: Delete failed for document {CollectionName}(Partition:{Partition}, id:{ResourceId}). Reason: {Reason}"
            );

        // SEARCH
        private static readonly Action<ILogger, string, string, Exception> searchRequested =
            LoggerMessage.Define<string, string>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbSearchRequested, nameof(EventCode.CosmosDbSearchRequested)),
                "CosmosDB: Search requested for document {CollectionName}(Partition:{Partition})"
            );

        private static readonly Action<ILogger, string, string, double, Exception> searchCompleted =
            LoggerMessage.Define<string, string, double>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbSearchCompleted, nameof(EventCode.CosmosDbSearchCompleted)),
                "CosmosDB: Search completed for document {CollectionName}(Partition:{Partition}). Request Charge: {RequestCharge}."
            );

        private static readonly Action<ILogger, string, string, string, Exception> searchFailed =
            LoggerMessage.Define<string, string, string>(
                LogLevel.Warning,
                new EventId((int)EventCode.CosmosDbSearchFailed, nameof(EventCode.CosmosDbSearchFailed)),
                "CosmosDB: Search failed for document {CollectionName}(Partition:{Partition}). Reason: {Reason}"
            );

        //SQLQuery
        private static readonly Action<ILogger, string, string, Exception> sqlQueryRequested =
            LoggerMessage.Define<string, string>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbSQLQueryRequested, nameof(EventCode.CosmosDbSQLQueryRequested)),
                "CosmosDB: SQLQuery requested for document {CollectionName}(Partition:{Partition})"
            );

        private static readonly Action<ILogger, string, string, double, Exception> sqlQueryCompleted =
            LoggerMessage.Define<string, string, double>(
                LogLevel.Information,
                new EventId((int)EventCode.CosmosDbSQLQueryCompleted, nameof(EventCode.CosmosDbSQLQueryCompleted)),
                "CosmosDB: SQLQuery completed for document {CollectionName}(Partition:{Partition}). Request Charge: {RequestCharge}."
            );

        private static readonly Action<ILogger, string, string, string, Exception> sqlQueryFailed =
            LoggerMessage.Define<string, string, string>(
                LogLevel.Warning,
                new EventId((int)EventCode.CosmosDbSQLQueryFailed, nameof(EventCode.CosmosDbSQLQueryFailed)),
                "CosmosDB: SQLQuery failed for document {CollectionName}(Partition:{Partition}). Reason: {Reason}"
            );

        //Exception

        /// <summary>
        /// When an exception is present in the failure, it will be logged as exception message instead of trace.
        /// Logging messages with an exception will make them an exception and the trace will lose an entry, making harder to debug issues
        /// </summary>
        private static void LogException(ILogger logger, Exception ex)
        {
            if (ex != null)
                logException(logger, ex.Message, ex);
        }

        // Initializing
        public static void Initializing(this ILogger logger, string containerName, string databaseName, string accountUri)
        {
            initializing(logger, containerName, databaseName, accountUri, null);
        }

        // GETById
        public static void GetByIdRequested(this ILogger logger, string containerName, string partitionKey, string itemId)
        {
            getByIdRequested(logger, containerName, partitionKey, itemId, null);
        }

        public static void GetByIdCompleted(this ILogger logger, string containerName, string partitionKey, string itemId, double requestCharge)
        {
            getByIdCompleted(logger, containerName, partitionKey, itemId, requestCharge, null);
        }

        public static void GetByIdFailed(this ILogger logger, string containerName, string partitionKey, string itemId, string reason, Exception ex)
        {
            getByIdFailed(logger, containerName, partitionKey, itemId, reason, null);
            LogException(logger, ex);
        }

        // Save
        public static void SaveRequested(this ILogger logger, string containerName, string partitionKey, string itemId)
        {
            saveRequested(logger, containerName, partitionKey, itemId, null);
        }

        public static void SaveCompleted(this ILogger logger, string containerName, string partitionKey, string itemId, double requestCharge)
        {
            saveCompleted(logger, containerName, partitionKey, itemId, requestCharge, null);
        }

        public static void SaveFailed(this ILogger logger, string containerName, string partitionKey, string itemId, string reason, Exception ex)
        {
            saveFailed(logger, containerName, partitionKey, itemId, reason, null);
            LogException(logger, ex);
        }

        // Delete
        public static void DeleteRequested(this ILogger logger, string containerName, string partitionKey, string itemId)
        {
            deleteRequested(logger, containerName, partitionKey, itemId, null);
        }

        public static void DeleteCompleted(this ILogger logger, string containerName, string partitionKey, string itemId, double requestCharge)
        {
            deleteCompleted(logger, containerName, partitionKey, itemId, requestCharge, null);
        }

        public static void DeleteFailed(this ILogger logger, string containerName, string partitionKey, string itemId, string reason, Exception ex)
        {
            deleteFailed(logger, containerName, partitionKey, itemId, reason, null);
            LogException(logger, ex);
        }

        // Search
        public static void SearchRequested(this ILogger logger, string containerName, string partitionKey)
        {
            searchRequested(logger, containerName, partitionKey, null);
        }

        public static void SearchCompleted(this ILogger logger, string containerName, string partitionKey, double requestCharge)
        {
            searchCompleted(logger, containerName, partitionKey, requestCharge, null);
        }

        public static void SearchFailed(this ILogger logger, string containerName, string partitionKey, string reason, Exception ex)
        {
            searchFailed(logger, containerName, partitionKey, reason, null);
            LogException(logger, ex);
        }

        // SQL Query
        public static void SQLQueryRequested(this ILogger logger, string containerName, string partitionKey)
        {
            sqlQueryRequested(logger, containerName, partitionKey, null);
        }

        public static void SQLQueryCompleted(this ILogger logger, string containerName, string partitionKey, double requestCharge)
        {
            sqlQueryCompleted(logger, containerName, partitionKey, requestCharge, null);
        }

        public static void SQLQueryFailed(this ILogger logger, string containerName, string partitionKey, string reason, Exception ex)
        {
            sqlQueryFailed(logger, containerName, partitionKey, reason, null);
            LogException(logger, ex);
        }
    }
}
#endif
