# This script is used to generate the appsettings.json files for the functional tests in the pipeline.
@(
  @{
    displayName = "FunctionalTests"
    template = "tests/xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests/appsettings.json"
    vars = @{
      cosmosdb_connection_string = "`${COSMOSDB_CONNECTION_STRING}"
      cosmosdb_name = "`${COSMOSDB_DATABASE_NAME}"
      cosmosdb_container_name = "`${COSMOSDB_CONTAINER_NAME}"
      servicebus_topic_name = "`${SERVICEBUS_TOPIC_NAME}"
      servicebus_subscription_name = "`${SERVICEBUS_SUBSCRIPTION_NAME}"
      servicebus_connectionstring = "`${SERVICEBUS_CONNECTIONSTRING}"
    }
  }
)
