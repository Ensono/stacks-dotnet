#!/bin/bash

IMAGE=`echo "${PWD##*/}" | tr '[:upper:]' '[:lower:]' | tr -d '\r'`; #current folder name
TAG=`date -u "+%Y.%m.%d-%H%M"`;

sh ./docker-build.sh $IMAGE $TAG

docker stop myapp
docker rm myapp
docker run -d -p 5005:80 --memory 150m --cpus=2 --name myapp $IMAGE:$TAG
