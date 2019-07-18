#!/bin/bash
set -x #echo on for commands

GITROOT=`git rev-parse --show-toplevel`
PROJNAME="$(basename $(git rev-parse --show-toplevel))"

IMAGE="$(whoami)/$PROJNAME-api"
TAG=`date -u "+%Y.%m.%d-%H%M"`
RELEASEDATE=`date --iso-8601=seconds`

#build
./docker-build.sh $IMAGE $TAG

#push
#./docker-push.sh $IMAGE $TAG

#temp folder
DEVFOLDER=$GITROOT/deploy/k8s/api/kustomization/localhost
rm -drf $DEVFOLDER  #clear temp folder before operation to avoid conflicts

#kustomize - replace variables
(
	if [ $# -eq 0 ]; then SOURCE=$GITROOT/deploy/k8s/api/base; else SOURCE=$GITROOT/deploy/k8s/api/kustomization/$1; fi;

	cp -r $SOURCE $DEVFOLDER
	cd $DEVFOLDER
	
	#echo "Source: $SOURCE"

	#set image
	kustomize edit set image menuapi-image=$IMAGE:$TAG;

	#set annotations
	kustomize edit add annotation version:$TAG;
	kustomize edit add annotation app.kubernetes.io/version:$TAG;
	kustomize edit add annotation release:0.0.1;
	kustomize edit add annotation releasedOn:$RELEASEDATE;

	#show output
	kubectl kustomize .

	#TODO: Improve the below script to dynamically get the namespace and deployment name without hardcoded values in the script
	# TMPYAML=`kubectl kustomize .`
	# echo $TMPYAML
	# TMPJSON=`kubectl kustomize . | kubectl create --dry-run -o JSON -f-`
	# echo $TMPJSON
	# TMPNAMESPACE=`kubectl kustomize . | grep -m 1 'namespace: ' | sed 's/namespace: //g' | xargs`
	# echo $TMPNAMESPACE
	
	#TODO: Make sure there is no deployment in progress
	
	#apply
	kubectl apply -k .
	
	#wait for deployment completion
	#TODO: modify the namespace and deployment based on current project
	kubectl rollout status -n default -w deploy/menuapi --timeout=30s
	if [ $? -eq 0 ]; then 
		echo "Deployment succeeded"; 
	else 
		echo "Deployment failed. Rolling back"; 
		kubectl rollout undo -n default deploy/menuapi
	fi;
	
)

#clean temp dev folder
rm -dr $DEVFOLDER
