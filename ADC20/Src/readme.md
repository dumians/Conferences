# Intro

The base Dockerfile provided uses ubuntu Linux. Vcpkg use with the x64-linux-musl triplet here is experimental. We have provided a Dockerfile.debian to show usage with a non experimental vcpkg triplet.

## Using the multistage container
Simple  

    docker build -t gcc_linux_docker:latest

This does result in a large intermdiate image that is unnamed. You may wish to reuse that image for additional development purposes, e.g. for preparing a container for use with VS. If so simply target the build stage and name it.

    docker build --target build -t gcc_linux_docker:latest.

Then that image will be cached and used when you prepare the run image.

    docker build -t  gcc_linux_docker:run .

To run the service:

     docker run --rm -it --name adc19 gcc_linux_docker:latest
    docker exec -it 8ad93e7148b8d08dffa83ea4a185940ef0e7dba06920e0d9e6e32f5bd715f248 /bin/sh -c "[ -e /bin/bash ] && /bin/bash || /bin/sh"

This will start the container detached and will delete it when it is stopped.

Attach to the container while running to watch process output.

    docker exec -it ggc_linux_docker

Or attach with an interactive session.

    docker exec --name adc19 -it gcc_linux_docker:latest /bin/bash

Stop the container

    docker stop gcc_linux_docker

## Service usage
Invoke the service with:

    curl -X PUT -T picture.jpg localhost:8080/files?submit=picture.jpg

Get the original image:

    curl -X GET localhost:8080/files/picture.jpg > picture.jpg

Get the modified image with faces circled (if none found it is the original image again):

    curl -X GET localhost:8080/files/facespicture.jpg > facespicture.jpg

The GET operations can also be performed by providing the url in a browser.

## Using the VS container

Building this container is simple.

    docker build -t findfaces/vs . -f Dockerfile.vs

We need to specify a bit more to run a container based on this image so VS can access it.

    docker run -d -p 12345:22 -p 8080:8080 --security-opt seccomp:unconfined --name findfacesvs findfaces/vs

To interact with the container while it is running, e.g. use the shell and poke around

    docker exec -it findfacesvs /bin/bash
