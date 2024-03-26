

dev-shell:
	bash -c "docker run --rm -it -v .:/home/ladesa/code -w /home/ladesa/code --user ladesa $$(docker build -q _devops/development)"

dev-run:
	bash -c "docker run --rm -it -v .:/home/ladesa/code -w /home/ladesa/code --user ladesa $$(docker build -q _devops/development) dotnet run --project dotnet-project"