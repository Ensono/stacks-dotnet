########
# Application level stuff will live here
# Each module is conditionally created within this app infra definition interface and can be re-used across app types e.g. SSR webapp, API only
########

# Naming convention
module "default_label" {
  source     = "git::https://github.com/cloudposse/terraform-null-label.git?ref=0.24.1"
  namespace  = "${var.name_company}-${var.name_project}"
  stage      = var.stage
  name       = "${lookup(var.location_name_map, var.resource_group_location, "uksouth")}-${var.name_domain}"
  attributes = var.attributes
  delimiter  = "-"
  tags       = var.tags
}

module "app" {
  source                               = "git::https://github.com/ensono/stacks-terraform//azurerm/modules/azurerm-server-side-app?ref=v6.0.27"
  create_cosmosdb                      = var.create_cosmosdb
  resource_namer                       = module.default_label.id
  resource_tags                        = module.default_label.tags
  resource_group_location              = var.resource_group_location
  cosmosdb_sql_container               = var.cosmosdb_sql_container
  cosmosdb_sql_container_partition_key = var.cosmosdb_sql_container_partition_key
  cosmosdb_kind                        = var.cosmosdb_kind
  cosmosdb_offer_type                  = var.cosmosdb_offer_type
  create_cache                         = var.create_cache
  create_dns_record                    = var.create_dns_record
  dns_record                           = var.dns_record
  dns_zone_name                        = data.terraform_remote_state.core.outputs.dns_base_domain
  dns_zone_resource_group              = data.terraform_remote_state.core.outputs.dns_resource_group_name != "" ? data.terraform_remote_state.core.outputs.dns_resource_group_name : data.terraform_remote_state.core.outputs.resource_group_name
  dns_ip_address_name                  = var.public_ip_name
  dns_ip_address_resource_group        = var.core_resource_group
  subscription_id                      = data.azurerm_client_config.current.subscription_id
  create_cdn_endpoint                  = var.create_cdn_endpoint
  force_create_resource_group          = var.create_resource_group
  # Alternatively if you want you can pass in the IP directly and remove the need for a lookup
  # dns_a_records                        = ["0.1.23.45"]
}

module "servicebus" {
  count                   = anytrue([var.create_sb_namespace, var.create_sb_topic, var.create_sb_subscription]) ? 1 : 0
  source                  = "../servicebus"
  resource_group_name     = var.sb_resource_group_name != "" ? var.sb_resource_group_name : module.app.resource_group
  resource_group_location = var.resource_group_location
  create_sb_namespace     = var.create_sb_namespace
  create_sb_topic         = var.create_sb_topic
  create_sb_subscription  = var.create_sb_subscription
  sb_name                 = var.sb_name != "" ? var.sb_name : null
  sb_topic_name           = var.sb_topic_name != "" ? var.sb_topic_name : null
  sb_subscription_name    = var.sb_subscription_name != "" ? var.sb_subscription_name : null
  sb_subscription_filter  = var.sb_subscription_filter
  sb_namespace_id         = local.lookup_servicebus_namespace ? data.azurerm_servicebus_namespace.sb[0].id : null
  sb_topic_id             = local.lookup_servicebus_topic ? data.azurerm_servicebus_topic.sb_topic[0].id : null
}

module "eventhub" {
  count                   = var.create_eventhub ? 1 : 0
  source                  = "../eventhub"
  resource_group_name     = module.app.resource_group
  resource_group_location = var.resource_group_location
}

module "function" {
  count                            = var.create_function_app ? 1 : 0
  source                           = "../function"
  function_name                    = var.function_name
  resource_group_name              = module.app.resource_group
  resource_group_location          = var.resource_group_location
  cosmosdb_database_name           = local.cosmosdb_account_name != "" ? local.cosmosdb_account_name : null
  cosmosdb_collection_name         = var.cosmosdb_sql_container != "" ? var.cosmosdb_sql_container : null
  cosmosdb_lease_collection_name   = var.create_cosmosdb || local.lookup_cosmosdb_account ? var.cosmosdb_lease_collection_name : null
  cosmosdb_connection_string       = var.create_cosmosdb || local.lookup_cosmosdb_account ? local.cosmosdb_connection_string : null
  sb_topic_name                    = local.sb_topic_name
  sb_connection_string             = local.sb_connection_string
  sb_subscription_name             = var.create_sb_subscription ? module.servicebus[0].servicebus_subscription_name : null
  eventhub_connection_string       = local.eventhub_connection_string
  eventhub_name                    = local.eventhub_name
  eventhub_storage_access_key      = local.eventhub_storage_access_key
  eventhub_blob_container          = local.eventhub_blob_container
  app_insights_connection_string   = data.azurerm_application_insights.example.connection_string
  app_insights_instrumentation_key = data.azurerm_application_insights.example.instrumentation_key
}
