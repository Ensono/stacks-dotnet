using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace xxAMIDOxx.xxSTACKSxx.Worker
{
    public class ChangeFeedListener
    {
        private readonly ILogger<ChangeFeedListener> logger;

        public ChangeFeedListener(ILogger<ChangeFeedListener> logger)
        {
            this.logger = logger;
        }

        [FunctionName(Constants.FunctionNames.CosmosDbChangeFeedListener)]
        public void Run([CosmosDBTrigger(
            databaseName: "SampleDB",
            collectionName: "Persons",
            ConnectionStringSetting = "CosmosDbConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input)
        {
            if (input != null && input.Count > 0)
            {
                logger.LogInformation("Documents modified " + input.Count);
                logger.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
