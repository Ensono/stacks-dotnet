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
  value       = azurerm_servicebus_subscription.sb_sub.name
}

output "servicebus_subscription_id" {
  description = "Servicebus Subscription ID"
  value       = azurerm_servicebus_subscription.sb_sub.id
}
