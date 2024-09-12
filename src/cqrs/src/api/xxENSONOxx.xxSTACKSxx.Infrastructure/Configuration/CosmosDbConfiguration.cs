#if CosmosDb
using System;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;

public class CosmosDbConfiguration
{
    public string DatabaseAccountUri { get; set; }
    public string DatabaseName { get; set; }
    public Secret SecurityKeySecret { get; set; }
    public string PrimaryKey { get; set; }
        
    /// <summary>
    /// Max query concurrency:
    ///  -1 = Dynamic, defined by server
    ///   0 = Serial
    ///  +1 = 1 or more dedicated asynchronous tasks to continuously make REST calls, returns out of order results
    /// </summary>
    public int MaxConcurrency { get; set; } = 1;

    /// <summary>
    /// Max results returned per batch when result size is not specified
    /// Default to a small number to save RU's, should be passed in the query if need bigger of by configuration
    /// </summary>
    public int MaxItemCountPerBatch { get; set; } = 20;

    public bool AllowSynchronousQueryExecution { get; set; } = false;

    public int? MaxBufferedItemCount { get; set; }

    /// <summary>
    /// Timeout for Non-Media operations (i.e: Get container or database information, not used for document operations. )
    /// Media operations(Get document, save, etc) are hard coded in the SDK to 5 minutes
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);
}
#endif
