#!/bin/bash
set -x #echo on for commands

GITROOT=`git rev-parse --show-toplevel`
PROJNAME="$(basename $(git rev-parse --show-toplevel))"

IMAGE="$(whoami)/$PROJNAME-api"
TAG=`date -u "+%Y.%m.%d-%H%M"`
RELEASEDATE=`date --iso-8601=seconds`

ENVNAME=$1

#build
./docker-build.sh $IMAGE $TAG

#push
#./docker-push.sh $IMAGE $TAG

#temp folder
DEVFOLDER=$GITROOT/deploy/k8s/api/kustomization/localhost
rm -drf $DEVFOLDER  #clear temp folder before operation to avoid conflicts

if [ $# -eq 0 ]; then NAMESPACE=default; else NAMESPACE=$ENVNAME-menu; fi;

#kustomize - replace variables
(
	if [ $# -eq 0 ]; then SOURCE=$GITROOT/deploy/k8s/api/base; else SOURCE=$GITROOT/deploy/k8s/api/kustomization/$ENVNAME; fi;

	cp -r $SOURCE $DEVFOLDER
	cd $DEVFOLDER
	
	#Make sure there is no deployment in progress, TODO: need to check if exit first otherwise it fails	
	#kubectl rollout status -n $NAMESPACE -w deploy/menuapi --timeout=30s
	#if [ $? -ne 0 ]; then exit 1; fi;
	
	#set image
	kustomize edit set image api-image=$IMAGE:$TAG;

	#set annotations
	kustomize edit add annotation version:$TAG;
	kustomize edit add annotation app.kubernetes.io/version:$TAG;
	kustomize edit add annotation release:0.0.1;
	kustomize edit add annotation releasedOn:$RELEASEDATE;

	#show generated YAML being applied
	kubectl kustomize .

	#apply changes
	kubectl apply -k .
	
	#wait for deployment completion
	#TODO: modify the deployment name based on current project, fro menuapi to api
	kubectl rollout status -n $NAMESPACE -w deploy/menuapi --timeout=30s
	if [ $? -eq 0 ]; then 
		echo "Deployment succeeded"; 
	else 
		echo "Deployment failed. Rolling back"; 
		kubectl rollout undo -n $NAMESPACE deploy/menuapi
	fi;
	
)

#clean temp dev folder
rm -dr $DEVFOLDER
