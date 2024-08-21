#!/bin/bash
kube_context="$1"
namespace="$2"
chart_name="$3"
chart_path="$4"
values_file="$5"
set_key_values="$6"

echo "Kube context = $kube_context"
echo "Namespace = $namespace"
echo "Chart name = $chart_name"
echo "Chart path = $chart_path"
echo "Values file = $values_file"
echo "Set key/value pairs = $set_key_values"

helm upgrade $chart_name --kube-context $kube_context --namespace $namespace $chart_path --install --values $values_file --set $set_key_values 
