#!/bin/bash

cwd=$(pwd)

docker build \
    -t mrdekk/maker \
    .
