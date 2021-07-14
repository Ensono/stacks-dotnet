#!/bin/bash

if [ $# -eq 0 ]; then PORT=5005; else PORT=$1; fi;

URL="http://localhost:${PORT}/api/values"
echo "Testing $URL"

ab.exe -n 1000000 -c 100 -t 180 -s 5  $URL