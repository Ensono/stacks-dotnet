GITROOT=`git rev-parse --show-toplevel`

#build
./docker-build.sh

TAG=`date -u "+%Y.%m.%d-%H%M"`
RELEASEDATE=`date --iso-8601=seconds`

#push
./docker-push.sh

#kustomize - replace variables

DEVFOLDER=$GITROOT/deploy/k8s/api/localhost

rm -drf $DEVFOLDER  #clear temp folder before operation to avoid conflicts

(
	cp -r $GITROOT/deploy/k8s/api/base $DEVFOLDER
	cd $DEVFOLDER
	
	#set image
	kustomize edit set image menuapi-image=amidostacksacrukstmp.azurecr.io/guibirow/xx-amido-xx.xx-stacks-xx.api:$TAG;

	#set annotations
	kustomize edit add annotation version:$TAG;
	kustomize edit add annotation app.kubernetes.io/version:$TAG;
	kustomize edit add annotation release:0.0.1;
	kustomize edit add annotation releasedOn:$RELEASEDATE;

	#apply
	kubectl apply -k .
)

#clean temp dev folder
rm -dr $DEVFOLDER
