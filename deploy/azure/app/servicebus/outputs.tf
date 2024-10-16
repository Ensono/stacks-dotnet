output "servicebus_namespace" {
  description = "Service bus namespace"
  value       = var.create_sb_namespace ? azurerm_servicebus_namespace.sb[0].name : null
}

output "servicebus_topic_name" {
  description = "Name of the topic"
  value       = var.create_sb_topic ? azurerm_servicebus_topic.sb_topic[0].name : null
}

output "servicebus_topic_id" {
  description = "ID of the topic"
  value       = var.create_sb_topic ? azurerm_servicebus_topic.sb_topic[0].id : null
}

output "servicebus_connectionstring" {
  value     = var.create_sb_namespace ? azurerm_servicebus_namespace.sb[0].default_primary_connection_string : null
  sensitive = true
}
