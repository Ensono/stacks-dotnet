output "function_id" {
  description = "Function ID"
  value       = azurerm_linux_function_app.function.id
}

output "function_name" {
  description = "Function name"
  value       = azurerm_linux_function_app.function.name
}
