locals {
  common_secrets = [
    {
      name  = "app-insights-key"
      value = "InstrumentationKey=${data.terraform_remote_state.core.outputs.instrumentation_key}"
    }
  ]

  cosmos_db_secrets = data.terraform_remote_state.app.outputs.cosmosdb_endpoint == "" ? [] : [
    {
      name  = "cosmosdb-key"
      value = data.terraform_remote_state.app.outputs.cosmosdb_primary_master_key
    }
  ]

  service_bus_secrets = var.servicebus_type == "" ? [] : [
    {
      name  = "servicebus-connection-string"
      value = data.terraform_remote_state.app.outputs.servicebus_connectionstring
    }
  ]

  container_app_secrets = concat(
    local.common_secrets,
    local.cosmos_db_secrets,
  local.service_bus_secrets)

  common_environment_variables = [{
    name  = "API_BASEPATH"
    value = var.app_route
    },
    {
      name  = "LOG_LEVEL"
      value = var.log_level
    },
    {
      name        = "APPLICATIONINSIGHTS_CONNECTION_STRING"
      secret_name = "app-insights-key"
    }
  ]

  cosmos_db_environment_variable = data.terraform_remote_state.app.outputs.cosmosdb_endpoint == "" ? [] : [
    {
      name        = "COSMOSDB_KEY"
      secret_name = "cosmosdb-key"
    },
    {
      name  = "CosmosDb__DatabaseAccountUri"
      value = data.terraform_remote_state.app.outputs.cosmosdb_endpoint
    },
    {
      name  = "CosmosDb__DatabaseName"
      value = data.terraform_remote_state.app.outputs.cosmosdb_database_name
    },
    {
      name  = "CosmosDb__SecurityKeySecret__Identifier"
      value = "COSMOSDB_KEY"
    },
    {
      name  = "CosmosDb__SecurityKeySecret__Source"
      value = "Environment"
    },
  ]

  service_bus_environment_variables = var.servicebus_type == "" ? [] : [
    {
      name        = "SERVICEBUS_CONNECTIONSTRING"
      secret_name = "servicebus-connection-string"
    }
  ]

  service_bus_sender_environment_variable = var.servicebus_type == "sender" ? [
    {
      name  = "Sender__Topics__0__Name"
      value = data.terraform_remote_state.app.outputs.servicebus_topic_name
    },
    {
      name  = "Sender__Topics__0__ConnectionStringSecret_Identifier"
      value = "SERVICEBUS_CONNECTIONSTRING"
    },
    {
      name  = "Sender__Topics__0__ConnectionStringSecret_Source"
      value = "Environment"
    }
  ] : []

  service_bus_listener_environment_variable = var.servicebus_type == "listener" ? [
    {
      name  = "Listener__Topics__0__Name"
      value = data.terraform_remote_state.app.outputs.servicebus_topic_name
    },
    {
      name  = "Listener__Topics__0__SubscriptionName"
      value = data.terraform_remote_state.app.outputs.servicebus_subscription_name
    },
    {
      name  = "Listener__Topics__0__ConcurrencyLevel"
      value = 5
    },
    {
      name  = "Listener__Topics__0__DisableProcessing"
      value = false
    },
    {
      name  = "Listener__Topics__0__DisableMessageValidation"
      value = true
    },
    {
      name  = "Listener__Topics__0__ConnectionStringSecret__Identifier"
      value = "SERVICEBUS_CONNECTIONSTRING"
    },
    {
      name  = "Listener__Topics__0__ConnectionStringSecret__Source"
      value = "Environment"
    },
  ] : []

  environment_variables = concat(
    local.common_environment_variables,
    local.cosmos_db_environment_variable,
    local.service_bus_environment_variables,
    local.service_bus_sender_environment_variable,
  local.service_bus_listener_environment_variable)
}
