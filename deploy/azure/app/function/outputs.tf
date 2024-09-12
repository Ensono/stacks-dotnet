output "servicebus_subscription_name" {
  description = "Servicebus Subscription name"
  value       = azurerm_servicebus_subscription.sb_sub.name
}

output "servicebus_subscription_id" {
  description = "Servicebus Subscription ID"
  value       = azurerm_servicebus_subscription.sb_sub.id
}
