# LOGA
Learn Old Georgian Alphabet

## Deploy
* ```docker login``` (in PS console, hubua/*** )
* Publish image to Docker Cloud (https://hub.docker.com/r/hubua/loga_webui/ or from VS Publish)
* ssh to remote server
* ```docker pull hubua/loga_webui```
* ```docker stop``` running containers "loga" and "nginx"
* ```nano docker-compose-loga-nginx.yml``` Update image tag in docker-compose-loga-nginx.yml
* ```sudo docker-compose -f docker-compose-loga-nginx.yml up -d```

## Usefull
* https://developers.google.com/analytics/solutions/experiments-client-side
* https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters#exception-filters
