if [ $# -eq 0 ]; then TAG=`date -u "+%Y.%m.%d-%H%M"`; else TAG=$1; fi;

IMAGE=`echo "${PWD##*/}" | tr '[:upper:]' '[:lower:]' | tr -d '\r'`

echo "Building image $IMAGE:$TAG"

docker build -t $IMAGE:$TAG .
