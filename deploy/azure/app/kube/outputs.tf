# CosmosDB Configuration
output "cosmosdb_database_name" {
  description = "CosmosDB Database name"
  value       = var.create_cosmosdb || local.lookup_cosmosdb_account ? local.cosmosdb_account_name : null
}

output "cosmosdb_account_name" {
  description = "CosmosDB account name"
  value       = var.create_cosmosdb || local.lookup_cosmosdb_account ? local.cosmosdb_account_name : null
}

output "cosmosdb_container_name" {
  description = "CosmosDB Container name"
  value       = var.cosmosdb_sql_container != "" ? var.cosmosdb_sql_container : null
}

output "cosmosdb_endpoint" {
  description = "Endpoint for accessing the DB CRUD"
  value       = var.create_cosmosdb || local.lookup_cosmosdb_account ? local.cosmosdb_endpoint : null
}

output "cosmosdb_primary_master_key" {
  description = "Primary Key for accessing the DB CRUD, should only be used in applications running outside of AzureCloud"
  sensitive   = true
  value       = var.create_cosmosdb ? module.app.cosmosdb_primary_master_key : null
}

output "cosmosdb_connection_string" {
  description = "Connection string for accessing the DB CRUD"
  sensitive   = true
  value       = var.create_cosmosdb || local.lookup_cosmosdb_account ? local.cosmosdb_connection_string : null
}

# Redis Configuration
output "redis_cache_key" {
  description = "Primary Key for accessing the RedisCache, should only be used in applications running outside of AzureCloud"
  sensitive   = true
  value       = module.app.redis_cache_key
}

output "redis_cache_hostname" {
  description = "Primary Hostname endpoint for Redis Cache"
  value       = module.app.redis_cache_hostname
}

# Azure Configuration
output "resource_group" {
  description = "Resource group name for the app"
  value       = module.app.resource_group
}

output "dns_name" {
  description = "DNS Name if created"
  value       = module.app.dns_name
}

# Events - Servicebus
output "servicebus_namespace" {
  description = "Service bus namespace"
  value       = var.create_sb_namespace ? module.servicebus.*.servicebus_namespace[0] : null
}

output "servicebus_topic_name" {
  description = "Name of the topic"
  value       = local.sb_topic_name
}

output "servicebus_connectionstring" {
  value     = local.sb_connection_string
  sensitive = true
}

output "servicebus_subscription_id" {
  description = "Servicebus Subscription ID"
  value       = var.create_sb_subscription ? module.servicebus.*.servicebus_subscription_id[0] : null
}

output "servicebus_subscription_name" {
  description = "Servicebus Subscription name"
  value       = var.create_sb_subscription ? module.servicebus.*.servicebus_subscription_name[0] : null
}

# Events - Function
output "function_id" {
  description = "Function App ID"
  value       = var.create_function_app ? module.function.*.function_id[0] : null
}

output "function_name" {
  description = "Function App Name"
  value       = var.create_function_app ? module.function.*.function_name[0] : null
}


# Events - Eventhub
output "eventhub_connectionstring" {
  value     = var.create_eventhub ? module.eventhub.*.eventhub_connectionstring[0] : null
  sensitive = true
}
output "eventhub_name" {
  value = var.create_eventhub ? module.eventhub.*.eventhub_name[0] : null
}

output "eventhub_sa_connectionstring" {
  value     = var.create_eventhub ? module.eventhub.*.eventhub_sa_connectionstring[0] : null
  sensitive = true
}

output "eventhub_sa_container" {
  value = var.create_eventhub ? module.eventhub.*.eventhub_sa_container[0] : null
}

# Core State Query Outputs (to reduce variable duplication)
output "dns_base_domain" {
  description = "Name of the base domain for core DNS"
  value       = data.terraform_remote_state.core.outputs.dns_base_domain
}

#TODO: This output should be removed
output "aks_resource_group_name" {
  description = "Name of the Resource Group in which the K8s cluster is deployed"
  value       = var.deploy_to_aca ? null : data.terraform_remote_state.core.outputs.aks_resource_group_name
}

#TODO: This output should be removed
output "aks_cluster_name" {
  description = "Name of the AKS cluster"
  value       = var.deploy_to_aca ? null : data.terraform_remote_state.core.outputs.aks_cluster_name
}

output "resource_group_name" {
  description = "Name of the core resource group"
  value       = data.terraform_remote_state.core.outputs.resource_group_name
}

output "acr_resource_group_name" {
  description = "Name of the resource group the container registry belongs to"
  value       = data.terraform_remote_state.core.outputs.acr_resource_group_name
}

output "acr_registry_name" {
  description = "Name of the Docker registry to push images to"
  value       = data.terraform_remote_state.core.outputs.acr_registry_name
}

output "app_insights_name" {
  description = "Name of the Application Insights instance"
  value       = data.terraform_remote_state.core.outputs.app_insights_name
}

output "app_insights_instrumentation_key" {
  description = "App Insights key for downstream deploymnent use"
  value       = data.azurerm_application_insights.example.instrumentation_key
  sensitive   = true
}

output "app_gateway_ip" {
  description = "IP address of the Application Gateway"
  value       = data.terraform_remote_state.core.outputs.app_gateway_ip
}
