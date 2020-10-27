#!/bin/bash

cwd=$(pwd)

docker stop maker
docker rm maker
docker run \
	-dt \
	--name maker \
	-p 6666:6666 \
	-v ${cwd}:/opt \
	--privileged \
	mrdekk/maker \
    "${@}" 

