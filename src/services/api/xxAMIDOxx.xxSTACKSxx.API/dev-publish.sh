#!/bin/bash

GITROOT=`git rev-parse --show-toplevel`

IMAGE='amidostacksacrukstmp.azurecr.io/guibirow/xx-amido-xx.xx-stacks-xx.api'
TAG=`date -u "+%Y.%m.%d-%H%M"`
RELEASEDATE=`date --iso-8601=seconds`

#build
./docker-build.sh $IMAGE $TAG

#push
./docker-push.sh $IMAGE $TAG

#temp folder
DEVFOLDER=$GITROOT/deploy/k8s/api/kustomization/localhost
rm -drf $DEVFOLDER  #clear temp folder before operation to avoid conflicts

#kustomize - replace variables
(
	if [ $# -eq 0 ]; then SOURCE=$GITROOT/deploy/k8s/api/base; else SOURCE=$GITROOT/deploy/k8s/api/kustomization/$1; fi;

	cp -r $SOURCE $DEVFOLDER
	cd $DEVFOLDER
	
	echo "Source: $SOURCE"

	#set image
	kustomize edit set image menuapi-image=$IMAGE:$TAG;

	#set annotations
	kustomize edit add annotation version:$TAG;
	kustomize edit add annotation app.kubernetes.io/version:$TAG;
	kustomize edit add annotation release:0.0.1;
	kustomize edit add annotation releasedOn:$RELEASEDATE;

	#show output
	kubectl kustomize .
	#apply
	kubectl apply -k .
)

#clean temp dev folder
rm -dr $DEVFOLDER
