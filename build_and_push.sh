#!/bin/bash
docker build -t isitar/timetracking-api -f Dockerfile.api .
docker build -t isitar/timetracking-frontend -f Dockerfile.frontend .

docker push isitar/timetracking-api
docker push isitar/timetracking-frontend

