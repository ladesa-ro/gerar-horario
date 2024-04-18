cold-start:
	make stop;
	make start;

start:
	docker compose build -q;
	docker compose up -d;

stop:
	docker compose down -v;

logs:
	docker compose logs lab -f;

shell:
	make start;
	docker compose exec -it lab bash;

open-linux:
	nohup xdg-open http://127.0.0.1:8888/tree &>/dev/null