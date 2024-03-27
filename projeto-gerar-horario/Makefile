build:
	dotnet build

clean:
	dotnet clean

restore:
	dotnet restore

test:
	dotnet test
start:
	dotnet run --project GerarHorario-Playground/

docker-shell:
	docker build ../_devops/dotnet-sdk;

	bash -c "docker run --rm -it -v .:/home/ladesa/sisgea -w /home/ladesa/sisgea --user ladesa $$(docker build -q ../_devops/dotnet-sdk) bash"
