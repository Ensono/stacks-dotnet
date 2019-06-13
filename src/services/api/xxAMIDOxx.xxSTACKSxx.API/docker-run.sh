IMAGE=`echo "${PWD##*/}" | tr '[:upper:]' '[:lower:]' | tr -d '\r'`;

docker stop myapp
docker rm myapp
#docker run -d -p 5005:80 --name myapp $IMAGE
docker run -d -p 5005:80 --memory 50m --cpus=2 --name myapp $IMAGE