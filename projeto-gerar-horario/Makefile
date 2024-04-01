build:
	dotnet build

clean:
	dotnet clean

restore:
	dotnet restore

test:
	dotnet test

start:
	echo "Nenhum projeto do tipo Console para iniciar."
	exit 1;

docker-shell:
	docker build ./.devops/dotnet-sdk;

	bash -c "docker run --rm -it -v .:/home/ladesa/sisgea -w /home/ladesa/sisgea --user ladesa $$(docker build -q ./.devops/dotnet-sdk) bash"
