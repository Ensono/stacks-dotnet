#Params to be passed via command line args

REGISTRY="amidostacksacrukstmp.azurecr.io"
REPO=`whoami`
IMAGE="xx-amido-xx.xx-stacks-xx.api"
TAG=`date -u "+%Y.%m.%d-%H%M"`

# Actions
echo "Tagging image $IMAGE as $REGISTRY/$REPO/$IMAGE:$TAG"
docker tag $IMAGE $REGISTRY/$REPO/$IMAGE:$TAG

echo ""

echo "Pushing image to registry: $REGISTRY/$REPO/$IMAGE:$TAG"
docker push $REGISTRY/$REPO/$IMAGE:$TAG
