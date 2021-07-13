#!/bin/bash

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

echo "Building image $IMAGE:$TAG"

docker build -t $IMAGE:$TAG .
