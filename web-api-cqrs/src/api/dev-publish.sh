#!/bin/bash

git status > /dev/null
[ $? -ne 0 ] && echo "This command must be executed inside a git repository." && exit 1;

GITROOT=`git rev-parse --show-toplevel`
PROJNAME="$(basename $(git rev-parse --show-toplevel) | tr '[:upper:]' '[:lower:]')"

IMAGE="$(whoami)/$PROJNAME-api"
TAG=`date -u "+%Y.%m.%d-%H%M"`
RELEASEDATE=`date --iso-8601=seconds`

ENVNAME=$1

DEPLOYMENT="$(grep -m 1 'name' $GITROOT/deploy/k8s/api/base/deployment.yaml | sed -e 's/name://' -e 's/\s*//' | tr -d '\r')"

if [ $# -eq 0 ]; 
then 
	NAMESPACE=default; 
	echo "Please provide an environment name. Example: $0 dev"
	exit; #don't want to use default yet
else 
	NAMESPACE="$(grep -m 1 'name' $GITROOT/deploy/k8s/api/kustomization/$ENVNAME/namespace.yaml | sed -e 's/name://' -e 's/\s*//'  -e 's/\r*//')"
fi;

#to check if there is not whitespace
echo "-$NAMESPACE-"
echo "-$DEPLOYMENT-"
#exit

#temp folder
DEVFOLDER=$GITROOT/deploy/k8s/api/kustomization/localhost
rm -drf $DEVFOLDER  #clear temp folder before operation to avoid conflicts

#build
./docker-build.sh $IMAGE $TAG

#push
#./docker-push.sh $IMAGE $TAG

populate_secrets ()
{
	. ./load-env-file.sh .env
	. ./inject-secrets-from-env-var.sh $1 #todo: refactor to use a variable instead
}

#kustomize - replace variables
(
	if [ $# -eq 0 ]; then SOURCE=$GITROOT/deploy/k8s/api/base; else SOURCE=$GITROOT/deploy/k8s/api/kustomization/$ENVNAME; fi;

	cp -r $SOURCE $DEVFOLDER
	
	populate_secrets "$DEVFOLDER/secrets"
	
	cd $DEVFOLDER
	
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
	kubectl rollout status -n $NAMESPACE deploy/$DEPLOYMENT  -w --timeout=30s
	if [ $? -eq 0 ]; then 
		echo "Deployment succeeded"; 
	else 
		echo "Deployment failed. Rolling back";
		kubectl rollout undo -n $NAMESPACE deploy/$DEPLOYMENT
	fi;
	
)

#clean temp dev folder
rm -dr $DEVFOLDER
