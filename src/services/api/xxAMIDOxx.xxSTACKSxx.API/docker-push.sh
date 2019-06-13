#Params to be passed via command line args

REGISTRY="amidostacksacrukstmp.azurecr.io"
REPO=`whoami`
#IMAGE="xx-amido-xx.xx-stacks-xx.api"
#TAG=`date -u "+%Y.%m.%d-%H%M"`
if [ $# -eq 2 ]; then 
    IMAGE=$1; 
    TAG=$2;
elif [ $# -eq 0 ]; then 
    IMAGE=`echo "${PWD##*/}" | tr '[:upper:]' '[:lower:]' | tr -d '\r'`; 
    TAG=`date -u "+%Y.%m.%d-%H%M"`;
else
    echo "Invalid number of args. Use it without parameter to autogenerate values or pass image name and tag. i.e: '$(basename -- $0) imagename tag'"
    exit 1
fi;

# Actions
echo "Tagging image $IMAGE as $REGISTRY/$REPO/$IMAGE:$TAG"

docker tag $IMAGE $REGISTRY/$REPO/$IMAGE:latest
docker tag $IMAGE $REGISTRY/$REPO/$IMAGE:$TAG

echo ""

echo "Pushing image to registry: $REGISTRY/$REPO/$IMAGE:$TAG"
docker push $REGISTRY/$REPO/$IMAGE:$TAG
