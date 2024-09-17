resource "azurerm_servicebus_subscription" "sb_sub" {
  count    = var.sb_subscription_name != null && var.sb_topic_id != null ? 1 : 0
  name     = var.sb_subscription_name
  topic_id = var.sb_topic_id

  max_delivery_count = var.sb_max_delivery_count
}

resource "azurerm_servicebus_subscription_rule" "sb_sub_rule" {
  count           = var.sb_subscription_filter != "" ? 1 : 0
  name            = "${var.sb_subscription_name}-rule"
  subscription_id = azurerm_servicebus_subscription.sb_sub[0].id

  filter_type = "SqlFilter"
  sql_filter  = var.sb_subscription_filter
}