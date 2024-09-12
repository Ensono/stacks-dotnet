resource "azurerm_servicebus_subscription" "sb_sub" {
  name     = var.sb_subscription_name
  topic_id = var.sb_topic_id

  max_delivery_count = var.sb_max_delivery_count
}

resource "azurerm_servicebus_subscription_rule" "sb_sub_rule" {
  count           = var.sb_subscription_filter != "" ? 1 : 0
  name            = "${var.sb_subscription_name}-rule"
  subscription_id = azurerm_servicebus_subscription.sb_sub.id

  filter_type = "SqlFilter"
  sql_filter  = var.sb_subscription_filter
  #   sql_filter  = "enclosedmessagetype like '%MenuCreatedEvent%'"
}