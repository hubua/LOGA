# LOGA
Learn Old Georgian Alphabet

## Deploy
* `docker login` in local PS console ( hubua / *** )
* Publish image to Docker Cloud (from VS Publish to https://hub.docker.com/r/hubua/loga_webapp/)
* Update image tag to latest pulled in docker-compose yml `nano docker-compose-loga-nginx.yml`
* View running containers `docker ps`
* Stop running containers "loga" and "nginx" `docker stop {conainer NAME or ID}` or `sudo docker rm -f $(sudo docker ps -a -q)`
* Run multi-container `sudo docker-compose -f docker-compose-loga-nginx.yml up -d`

## Usefull
* https://developers.google.com/analytics/solutions/experiments-client-side
* `docker rmi -f $(docker images -a -q)`
