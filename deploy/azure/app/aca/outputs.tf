##########################
# Azure Container Apps
##########################
output "container_app_name" {
  description = "The name of the created Container App"
  value       = module.aca.container_app_name
}

output "container_app_id" {
  description = "The ID of the created Container App"
  value       = module.aca.container_app_id
}

output "container_app_fqdn" {
  description = "The FQDN of the created Container App"
  value       = module.aca.container_app_fqdn
}

output "container_app_principal_id" {
  description = "The Principal ID of the Container App's Managed Identity"
  value       = module.aca.container_app_principal_id
}
