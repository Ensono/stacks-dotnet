#!/bin/bash

azure_tenant_id="$1"
azure_subscription_id="$2"
azure_client_id="$3"
azure_client_secret="$4"

acr_registry_name_nonprod="$5"
acr_registry_name_prod="$6"

image_name="$7"
image_tag="$8"


image_name_nonprod="$acr_registry_name_nonprod.azurecr.io/$image_name:$image_tag"
image_name_prod="$acr_registry_name_prod.azurecr.io/$image_name:$image_tag"

echo "Azure Client ID = $azure_client_id"
echo "Azure Client Secret = $azure_client_secret"
echo "Azure Subscription ID = $azure_subscription_id"
echo "Azure Tenant ID = $azure_tenant_id"
echo "Azure ACR NonProd = $acr_registry_name_nonprod"
echo "Azure ACR Prod = $acr_registry_name_prod"
echo "Image name = $image_name"
echo "Image tag = $image_tag"

echo "Full image name NonProd = $image_name_nonprod"
echo "Full image name Prod = $image_name_prod"

echo "Logging in to Azure"
az login --service-principal --username $azure_client_id --password $azure_client_secret --tenant $azure_tenant_id
az account set -s $azure_subscription_id

echo "Logging in to Non-Production ACR '$acr_registry_name_nonprod'"
az acr login --name $acr_registry_name_nonprod

echo "Logging in to Production ACR '$acr_registry_name_prod'"
az acr login --name $acr_registry_name_prod

echo "Pulling image from nonprod repo"
docker pull $image_name_nonprod

docker tag $image_name_nonprod $image_name_prod

echo "Pushing image to prod repo"
docker push $image_name_prod