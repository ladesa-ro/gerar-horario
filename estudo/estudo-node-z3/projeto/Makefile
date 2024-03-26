
dev-shell:
	bash -c "docker run --rm -it -v .:/home/node/code -w /home/node/code --user node -it $$(docker build -q _devops/development) bash"

dev-run:
	bash -c "docker run --rm -it -v .:/home/node/code -w /home/node/code --user node -it $$(docker build -q _devops/development) bash -c 'clear && (npm i &>/dev/null) && clear && npm run dev-start'"
