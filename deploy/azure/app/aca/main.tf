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

  service_bus_secrets = var.servicebus_type == null ? [] : [
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

  service_bus_environment_variables = var.servicebus_type == null ? [] : [
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

# Naming convention
module "default_label" {
  source     = "git::https://github.com/cloudposse/terraform-null-label.git?ref=0.24.1"
  namespace  = "${var.name_company}-${var.name_project}"
  stage      = var.stage
  name       = "${lookup(var.location_name_map, var.resource_group_location, "uksouth")}-${var.name_domain}"
  attributes = var.attributes
  delimiter  = "-"
  tags       = var.tags
}

module "aca" {
  source = "git::https://github.com/Ensono/terraform-azurerm-aca?ref=1.0.4"

  resource_group_name = module.default_label.id
  location            = var.resource_group_location
  resource_tags = merge(module.default_label.tags,
    {
      cdm_task_configuration = "enabled=true"
    }
  )

  create_container_app_environment = false
  create_container_app             = true
  create_rg                        = false

  # Container App
  container_app_name = var.name_domain
  container_app_registry = {
    server   = "${data.azurerm_container_registry.acr.name}.azurecr.io"
    identity = azurerm_user_assigned_identity.default.id
  }
  container_app_identity = {
    type         = "UserAssigned",
    identity_ids = [azurerm_user_assigned_identity.default.id]
  }

  container_app_environment_id        = data.terraform_remote_state.core.outputs.acae_id
  container_app_workload_profile_name = "Consumption"
  container_app_revision_mode         = "Single"
  container_app_secrets               = local.container_app_secrets

  container_app_containers = [
    {
      cpu    = 0.25
      image  = "${data.azurerm_container_registry.acr.name}.azurecr.io/${var.docker_image_name}:${var.docker_image_tag}"
      memory = "0.5Gi"
      name   = var.name_domain
      env    = local.environment_variables
      volume_mounts = [
        # {
        #   name = "data"
        #   path = "/app/data"
        # }
      ]
    }

  ]

  container_app_container_max_replicas = 1
  container_app_container_min_replicas = 1
  # container_app_container_volumes      = [
  #   {
  #     name         = "data"
  #     storage_type = "EmptyDir"
  #   }
  # ]

  #  Ingress configuration
  container_app_ingress_external_enabled           = var.ingress_enabled
  container_app_ingress_target_port                = var.ingress_enabled ? var.ingress_port : null
  container_app_ingress_allow_insecure_connections = true
  create_custom_domain_for_container_app           = var.ingress_enabled # If we want to expose the service then we should create a custom domain for it so the app gateway can route to it
  custom_domain                                    = "${var.dns_record}.${data.terraform_remote_state.core.outputs.dns_base_domain}"
}

resource "azurerm_user_assigned_identity" "default" {
  resource_group_name = module.default_label.id
  location            = var.resource_group_location
  name                = module.default_label.id
  lifecycle {
    ignore_changes = [
      tags,
    ]
  }
}

resource "azurerm_role_assignment" "acrpull_role" {
  scope                            = data.azurerm_container_registry.acr.id
  role_definition_name             = "AcrPull"
  principal_id                     = azurerm_user_assigned_identity.default.principal_id
  skip_service_principal_aad_check = true
}
