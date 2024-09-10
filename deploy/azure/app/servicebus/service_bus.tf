
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

resource "azurerm_servicebus_subscription" "sb_sub_1" {
  name     = var.sb_subscription_name
  topic_id = azurerm_servicebus_topic.sb_topic.id

  max_delivery_count = var.sb_max_delivery_count
}

resource "azurerm_servicebus_subscription" "sb_sub_2" {
  name     = var.sb_subscription_filtered_name
  topic_id = azurerm_servicebus_topic.sb_topic.id

  max_delivery_count = var.sb_max_delivery_count
}

resource "azurerm_servicebus_subscription_rule" "sb_sub_2_rule" {
  name            = "${var.sb_subscription_filtered_name}-rule"
  subscription_id = azurerm_servicebus_subscription.sb_sub_2.id

  filter_type = "SqlFilter"
  sql_filter  = "enclosedmessagetype like '%MenuCreatedEvent%'"
}
