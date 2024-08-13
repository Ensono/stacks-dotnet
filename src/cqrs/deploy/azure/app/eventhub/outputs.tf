output "eventhub_connectionstring" {
  value     = azurerm_eventhub_namespace.eh_ns.default_primary_connection_string
  sensitive = true
}

output "eventhub_name" {
  value = azurerm_eventhub.eh.name
}

output "eventhub_sa_connectionstring" {
  value     = azurerm_storage_account.eh_storage.primary_connection_string
  sensitive = true
}

output "eventhub_sa_container" {
  value = azurerm_storage_container.eh_storage_container.name
}

output "function_listener_name" {
  value = azurerm_function_app.function_listener.name
}

output "function_listener_id" {
  value = azurerm_function_app.function_listener.id
}

