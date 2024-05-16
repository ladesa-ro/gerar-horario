#!/bin/bash


if [ $(docker image ls -f "reference=some-app" | wc -l) -gt 1 ]; 
	then echo "Imagem some-app já existe, pulando build"
else
	echo "Iniciando Build..."
	docker build -f "$(pwd)/Dockerfile.service" -t some-app .
fi

docker run --env-file "$(pwd)/Microsservicos/GerarHorario-Service/.env" some-app
