using System;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Common.Events;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Logging;

/// <summary>
/// Contains log definitions for DynamoDB component
/// LoggerMessage.Define() creates a unique template for each log type
/// The log template reduces the number of allocations and write logs faster to destination - https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging
/// </summary>
public static class DynamoDbLogDefinition
{
	/// Failures with exceptions should be logged to respective failures(i.e: getByIdFailed) and then to logException in order to show them as separate entries in the logs(trace + exception
	private static readonly Action<ILogger, string, Exception> logException =
		LoggerMessage.Define<string>(
			LogLevel.Error,
			new EventId((int)EventCode.DynamoDbGeneralException, nameof(EventCode.DynamoDbGeneralException)),
			"DynamoDB Exception: {Message}"
		);

	//GETById
	private static readonly Action<ILogger, string, Exception> getByIdRequested =
		LoggerMessage.Define<string>(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbGetByIdRequested, nameof(EventCode.DynamoDbGetByIdRequested)),
			"DynamoDB: GetById requested for document (Partition:{Partition})"
		);

	private static readonly Action<ILogger, string, Exception> getByIdCompleted =
		LoggerMessage.Define<string>(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbGetByIdCompleted, nameof(EventCode.DynamoDbGetByIdCompleted)),
			"DynamoDB: GetById completed for document (Partition:{Partition})"
		);

	private static readonly Action<ILogger, string, string, Exception> getByIdFailed =
		LoggerMessage.Define<string, string>(
			LogLevel.Warning,
			new EventId((int)EventCode.DynamoDbGetByIdFailed, nameof(EventCode.DynamoDbGetByIdFailed)),
			"DynamoDB: GetById failed for document (Partition:{Partition}). Reason: {Reason}"
		);

	//SAVE
	private static readonly Action<ILogger, string, Exception> saveRequested =
		LoggerMessage.Define<string>(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbSaveRequested, nameof(EventCode.DynamoDbSaveRequested)),
			"DynamoDB: Save requested for document (Partition:{Partition})"
		);

	private static readonly Action<ILogger, string, Exception> saveCompleted =
		LoggerMessage.Define<string>(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbSaveCompleted, nameof(EventCode.DynamoDbSaveCompleted)),
			"DynamoDB: Save completed for document (Partition:{Partition})"
		);

	private static readonly Action<ILogger, string, string, Exception> saveFailed =
		LoggerMessage.Define<string, string>(
			LogLevel.Warning,
			new EventId((int)EventCode.DynamoDbSaveFailed, nameof(EventCode.DynamoDbSaveFailed)),
			"DynamoDB: Save failed for document (Partition:{Partition}). Reason: {Reason}"
		);

	//DELETE
	private static readonly Action<ILogger, string, Exception> deleteRequested =
		LoggerMessage.Define<string>(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbDeleteRequested, nameof(EventCode.DynamoDbDeleteRequested)),
			"DynamoDB: Delete requested for document (Partition:{Partition})"
		);

	private static readonly Action<ILogger, string, Exception> deleteCompleted =
		LoggerMessage.Define<string>(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbDeleteCompleted, nameof(EventCode.DynamoDbDeleteCompleted)),
			"DynamoDB: Delete completed for document (Partition:{Partition}"
		);

	private static readonly Action<ILogger, string, string, Exception> deleteFailed =
		LoggerMessage.Define<string, string>(
			LogLevel.Warning,
			new EventId((int)EventCode.DynamoDbDeleteFailed, nameof(EventCode.DynamoDbDeleteFailed)),
			"DynamoDB: Delete failed for document (Partition:{Partition}). Reason: {Reason}"
		);


	// ScanAsync / QueryAsync
	private static readonly Action<ILogger, Exception> scanAsyncRequested =
		LoggerMessage.Define(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbScanAsyncRequested, nameof(EventCode.DynamoDbScanAsyncRequested)),
			"DynamoDB: ScanAsync requested for document"
		);

	private static readonly Action<ILogger, Exception> scanAsyncCompleted =
		LoggerMessage.Define(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbScanAsyncCompleted, nameof(EventCode.DynamoDbScanAsyncCompleted)),
			"DynamoDB: ScanAsync completed for document"
		);

	private static readonly Action<ILogger, string, Exception> scanAsyncFailed =
		LoggerMessage.Define<string>(
			LogLevel.Warning,
			new EventId((int)EventCode.DynamoDbScanAsyncFailed, nameof(EventCode.DynamoDbScanAsyncFailed)),
			"DynamoDB: ScanAsync failed for document. Reason: {Reason}"
		);

	private static readonly Action<ILogger, Exception> queryAsyncRequested =
		LoggerMessage.Define(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbQueryAsyncRequested, nameof(EventCode.DynamoDbQueryAsyncRequested)),
			"DynamoDB: QueryAsync requested for document"
		);

	private static readonly Action<ILogger, Exception> queryAsyncCompleted =
		LoggerMessage.Define(
			LogLevel.Information,
			new EventId((int)EventCode.DynamoDbQueryAsyncCompleted, nameof(EventCode.DynamoDbQueryAsyncCompleted)),
			"DynamoDB: QueryAsync completed for document"
		);

	private static readonly Action<ILogger, string, Exception> queryAsyncFailed =
		LoggerMessage.Define<string>(
			LogLevel.Warning,
			new EventId((int)EventCode.DynamoDbQueryAsyncFailed, nameof(EventCode.DynamoDbQueryAsyncFailed)),
			"DynamoDB: QueryAsync failed for document. Reason: {Reason}"
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

	// GETById
	public static void GetByIdRequested(this ILogger logger, string partitionKey)
	{
		getByIdRequested(logger, partitionKey, null!);
	}

	public static void GetByIdCompleted(this ILogger logger, string partitionKey)
	{
		getByIdCompleted(logger, partitionKey, null!);
	}

	public static void GetByIdFailed(this ILogger logger, string partitionKey, string reason, Exception ex)
	{
		getByIdFailed(logger, partitionKey, reason, null!);
		LogException(logger, ex);
	}

	// Save
	public static void SaveRequested(this ILogger logger, string partitionKey)
	{
		saveRequested(logger, partitionKey, null!);
	}

	public static void SaveCompleted(this ILogger logger, string partitionKey)
	{
		saveCompleted(logger, partitionKey, null!);
	}

	public static void SaveFailed(this ILogger logger, string partitionKey, string reason, Exception ex)
	{
		saveFailed(logger, partitionKey, reason, null!);
		LogException(logger, ex);
	}

	// Delete
	public static void DeleteRequested(this ILogger logger, string partitionKey)
	{
		deleteRequested(logger, partitionKey, null!);
	}

	public static void DeleteCompleted(this ILogger logger, string partitionKey)
	{
		deleteCompleted(logger, partitionKey, null!);
	}

	public static void DeleteFailed(this ILogger logger, string partitionKey, string reason, Exception ex)
	{
		deleteFailed(logger, partitionKey, reason, null!);
		LogException(logger, ex);
	}

	// ScanAsync / QueryAsync
	public static void ScanAsyncRequested(this ILogger logger)
	{
		scanAsyncRequested(logger, null!);
	}

	public static void ScanAsyncCompleted(this ILogger logger)
	{
		scanAsyncCompleted(logger, null!);
	}

	public static void ScanAsyncFailed(this ILogger logger, string reason, Exception ex)
	{
		scanAsyncFailed(logger, reason, null!);
		LogException(logger, ex);
	}

	public static void QueryAsyncRequested(this ILogger logger)
	{
		queryAsyncRequested(logger, null!);
	}

	public static void QueryAsyncCompleted(this ILogger logger)
	{
		queryAsyncCompleted(logger, null!);
	}

	public static void QueryAsyncFailed(this ILogger logger, string reason, Exception ex)
	{
		queryAsyncFailed(logger, reason, null!);
		LogException(logger, ex);
	}
}
