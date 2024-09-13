
# Create a random string to be used for the 

resource "azurerm_servicebus_namespace" "sb" {
  name                = "${var.sb_name}-${random_string.seed.result}"
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location
  sku                 = var.servicebus_sku
}

resource "azurerm_servicebus_topic" "sb_topic" {
  name         = var.sb_topic_name
  namespace_id = azurerm_servicebus_namespace.sb.id
}
