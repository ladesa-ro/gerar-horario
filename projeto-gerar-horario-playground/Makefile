# user=ladesa

# start:
# 	docker build --build-arg USER=$(user) _devops;

# 	docker run -it --rm -p 8888:8888 -u $(user) -v $(shell dirname $$(pwd)):/home/user/gerar-horario $$(docker build --build-arg USER=$(user) -q _devops);

start:
	docker build _devops;

	docker run -it --rm -p 8888:8888 -u user -v $(shell dirname $$(pwd)):/home/user/gerar-horario $$(docker build -q _devops);
