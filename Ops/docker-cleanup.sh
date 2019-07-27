#!/usr/bin/env bash

echo "Stopping all running containers..."
docker stop $(docker ps -aq)

echo "Cleaning all unneeded docker containers/images/volumes"
docker system prune -f && docker volume prune -f
