
# Create a random string to be used for the 

resource "azurerm_servicebus_namespace" "sb" {
  count               = var.create_sb_namespace ? 1 : 0
  name                = "${var.sb_name}-${random_string.seed.result}"
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location
  sku                 = var.servicebus_sku
}

resource "azurerm_servicebus_topic" "sb_topic" {
  count        = var.create_sb_topic ? 1 : 0
  name         = var.sb_topic_name
  namespace_id = var.create_sb_namespace ? azurerm_servicebus_namespace.sb[0].id : var.sb_namespace_id
}

resource "azurerm_servicebus_subscription" "sb_sub" {
  count    = var.create_sb_subscription ? 1 : 0
  name     = var.sb_subscription_name
  topic_id = var.create_sb_topic ? azurerm_servicebus_topic.sb_topic[0].id : var.sb_topic_id

  max_delivery_count = var.sb_max_delivery_count
}