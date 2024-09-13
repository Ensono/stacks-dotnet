output "servicebus_namespace" {
  description = "Service bus namespace"
  value       = azurerm_servicebus_namespace.sb.name
}

output "servicebus_topic_name" {
  description = "Name of the topic"
  value       = azurerm_servicebus_topic.sb_topic.name
}

output "servicebus_topic_id" {
  description = "ID of the topic"
  value       = azurerm_servicebus_topic.sb_topic.id
}

output "servicebus_connectionstring" {
  value     = azurerm_servicebus_namespace.sb.default_primary_connection_string
  sensitive = true
}
