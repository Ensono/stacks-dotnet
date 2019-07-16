#!/bin/bash

#Params to be passed via command line args
#IMAGE="xx-amido-xx.xx-stacks-xx.api"
#TAG=`date -u "+%Y.%m.%d-%H%M"`

if [ $# -eq 2 ]; then 
    IMAGE=$1; 
    TAG=$2;
elif [ $# -eq 0 ]; then 
    IMAGE=`echo "${PWD##*/}" | tr '[:upper:]' '[:lower:]' | tr -d '\r'`; #current folder name
    TAG=`date -u "+%Y.%m.%d-%H%M"`;
else
    echo "Invalid number of args. Use it without parameter to autogenerate values or pass image name and tag. i.e: '$(basename -- $0) imagename tag'"
    exit 1
fi;

# Actions
#echo "Tagging image $IMAGE as $IMAGE:$TAG"
#docker tag $IMAGE $IMAGE:latest
#docker tag $IMAGE $IMAGE:$TAG

echo ""

echo "Pushing image to registry: $IMAGE:$TAG"
docker push $IMAGE:$TAG
