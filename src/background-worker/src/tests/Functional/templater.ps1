# This script is used to generate the appsettings.json files for the functional tests in the pipeline.
@(
  @{
    displayName = "FunctionalTests"
    template = "tests/xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests/appsettings.json"
    vars = @{
      servicebus_topic_name = "`${SERVICEBUS_TOPIC_NAME}"
      servicebus_subscription_name = "`${SERVICEBUS_SUBSCRIPTION_NAME}"
      servicebus_connectionstring = "`${SERVICEBUS_CONNECTIONSTRING}"
    }
  }
)
