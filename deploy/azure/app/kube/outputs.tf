# CosmosDB Configuration
output "cosmosdb_database_name" {
  description = "CosmosDB Database name"
  value       = module.app.cosmosdb_database_name
}

output "cosmosdb_account_name" {
  description = "CosmosDB account name"
  value       = module.app.cosmosdb_account_name
}

output "cosmosdb_endpoint" {
  description = "Endpoint for accessing the DB CRUD"
  value       = module.app.cosmosdb_endpoint
}

output "cosmosdb_primary_master_key" {
  description = "Primary Key for accessing the DB CRUD, should only be used in applications running outside of AzureCloud"
  sensitive   = true
  value       = module.app.cosmosdb_primary_master_key
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

# Core State Query Outputs (to reduce variable duplication)

output "dns_base_domain" {
  description = "Name of the base domain for core DNS"
  value       = data.terraform_remote_state.core.outputs.dns_base_domain
}

output "aks_resource_group_name" {
  description = "Name of the Resource Group in which the K8s cluster is deployed"
  value       = data.terraform_remote_state.core.outputs.aks_resource_group_name
}

output "aks_cluster_name" {
  description = "Name of the AKS cluster"
  value       = data.terraform_remote_state.core.outputs.aks_cluster_name
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
