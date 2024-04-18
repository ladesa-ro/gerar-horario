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