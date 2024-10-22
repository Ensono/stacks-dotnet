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

# Output for functional tests
output "servicebus_topic_name" {
  description = "Name of the topic"
  value       = var.servicebus_type != null ? data.terraform_remote_state.app.outputs.servicebus_topic_name : null
}

output "servicebus_connectionstring" {
  value     = var.servicebus_type != null ? data.terraform_remote_state.app.outputs.servicebus_connectionstring : null
  sensitive = true
}

output "servicebus_subscription_name" {
  description = "Servicebus Subscription name"
  value       = var.servicebus_type == "listener" ? data.terraform_remote_state.app.outputs.servicebus_subscription_name : null
}
