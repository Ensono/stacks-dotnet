#build
./docker-build.sh

TAG=`date -u "+%Y.%m.%d-%H%M"`
RELEASEDATE=`date --iso-8601=seconds`

#push
./docker-push.sh

#kustomize - replace variables

(
	cd ../../../../deploy/k8s/api/base; 
	
	kustomize edit set image menuapi=amidostacksacrukstmp.azurecr.io/guibirow/xx-amido-xx.xx-stacks-xx.api:$TAG;
	
	kustomize edit add annotation version:$TAG;
	kustomize edit add annotation app.kubernetes.io/version:$TAG;
	kustomize edit add annotation release:0.0.1;
	kustomize edit add annotation releasedOn:$RELEASEDATE;
)

#apply
kubectl apply -k ../../../../deploy/k8s/api/base
