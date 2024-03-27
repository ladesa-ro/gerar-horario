###### gerar-horario

# playground

## Desenvolvimento

### Obter o código

```sh
git clone https://github.com/sisgha/gerar-horario.git;
cd gerar-horario/playground;
```

### Scripts do projeto

Para iniciar o servidor jupyter:

```sh
make start; # inicia o container com notebook jupyter usando o docker compose
```

Aplicação Jupyter: <http://127.0.0.1:8888/tree>.

---

Para parar o servidor jupyter:

```sh
make stop; # para e remove o container da aplicação jupyter
```

---

Para ver os logs da aplicação jupyter:

```sh
make logs; # logs do container usando o docker compose
```

---

Caso queria iniciar um shell dentro do container:

```sh
make shell; # abre o bash dentro do container
```

---
