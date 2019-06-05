resource "azurerm_resource_group" "rg" {
  name     = "${local.resource_group_name}"
  location = "${var.resource_group_location}"
  tags     = "${var.resource_group_tags}"
}

/* resource "azurerm_traffic_manager_profile" "traffic_manager" {
  name                = "${local.traffic_manager_name}"
  resource_group_name = "${azurerm_resource_group.rg.name}"

  traffic_routing_method = "Weighted"

  dns_config {
    relative_name = "${local.traffic_manager_name}"
    ttl           = 60
  }

  monitor_config {
    protocol = "http"
    port     = 80
    path     = "/"
  }

  tags = "${var.resource_group_tags}"
}

resource "azurerm_traffic_manager_endpoint" "endpoints" {
  count               = "${length(var.cluster_ingress_address)}"
  name                = "${element(keys(var.cluster_ingress_address), count.index)}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  profile_name        = "${azurerm_traffic_manager_profile.traffic_manager.name}"
  target              = "${var.name_company}-${var.name_platform}-${var.name_project}-${var.name_environment}-${element(keys(var.cluster_ingress_address), count.index)}.${var.dns_zone}"
  type                = "externalEndpoints"
  weight              = 100
}

resource "azurerm_dns_cname_record" "ingress_cname_record" {
  count               = "${length(var.cluster_ingress_address)}"
  name                = "${var.name_company}-${var.name_platform}-${var.name_project}-${var.name_environment}-${element(keys(var.cluster_ingress_address), count.index)}"
  zone_name           = "${var.dns_zone}"
  resource_group_name = "${var.dns_zone_resource_group_name}"
  ttl                 = "${var.dns_record_ttl_seconds}"
  record = "${var.cluster_ingress_address[element(keys(var.cluster_ingress_address), count.index)]}"
}

resource "azurerm_dns_cname_record" "tm_cname_record" {
  name                = "${local.dns_record_address_tm}"
  zone_name           = "${var.dns_zone}"
  resource_group_name = "${var.dns_zone_resource_group_name}"
  ttl                 = "${var.dns_record_ttl_seconds}"
  record              = "${azurerm_traffic_manager_profile.traffic_manager.fqdn}"
}
*/

resource "azurerm_cosmosdb_account" "cosmosdb" {
  name                = "${local.cosmosdb_account_name}"
  location            = "${var.resource_group_location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  offer_type          = "Standard"
  kind                = "MongoDB"

  consistency_policy {
    consistency_level = "Session"
  }


  failover_policy {
    location = "North Europe"
    priority = 0
  }

  tags = "${var.resource_group_tags}"
  
}