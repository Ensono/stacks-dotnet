#!/bin/bash

azure_tenant_id="$1"
azure_subscription_id="$2"
azure_client_id="$3"
azure_client_secret="$4"
resource_group_name="$5"
aks_cluster_name="$6"

echo "Azure Client ID = $azure_client_id"
echo "Azure Client Secret = $azure_client_secret"
echo "Azure Subscription ID = $azure_subscription_id"
echo "Azure Tenant ID = $azure_tenant_id"
echo "Resource group name = $resource_group_name"
echo "AKS cluster name = $aks_cluster_name"

echo "Logging in to Azure"
az login --service-principal --username $azure_client_id --password $azure_client_secret --tenant $azure_tenant_id
az account set -s $azure_subscription_id

echo "Getting cluster credentials"
az aks get-credentials --resource-group $resource_group_name --name $aks_cluster_name
