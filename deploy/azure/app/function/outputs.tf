output "function_id" {
  description = "Function ID"
  value       = azurerm_linux_function_app.function.id
}

output "function_name" {
  description = "Function name"
  value       = azurerm_linux_function_app.function.name
}

output "servicebus_subscription_name" {
  description = "Servicebus Subscription name"
  value       = var.sb_subscription_name != null && var.sb_topic_id != null ? azurerm_servicebus_subscription.sb_sub[0].name : null
}

output "servicebus_subscription_id" {
  description = "Servicebus Subscription ID"
  value       = var.sb_subscription_name != null && var.sb_topic_id != null ? azurerm_servicebus_subscription.sb_sub[0].id : null
}
