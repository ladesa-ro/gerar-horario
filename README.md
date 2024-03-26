# gerar-horario

Home (Allure Report): <https://sisgha.github.io/gerar-horario>.

[![CodeQL](https://github.com/sisgha/gerar-horario/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/sisgha/gerar-horario/actions/workflows/github-code-scanning/codeql)
[![NUnit Tests + Allure Reports](https://github.com/sisgha/gerar-horario/actions/workflows/test-and-deploy.yml/badge.svg)](https://github.com/sisgha/gerar-horario/actions/workflows/test-and-deploy.yml)


## Desenvolvimento

```sh
git clone https://github.com/sisgha/gerar-horario.git;
cd gerar-horario;
```


## Projeto Gerar Horário

```sh
cd projeto-gerar-horario;
```

```sh
dotnet test;
```

```sh
dotnet run --project Core.Playground;
```

## Estudo

### Estudo CSharp

```sh
cd estudo-csharp;
```

```sh
make dev-run; # dotnet run
```

```sh
make dev-shell; # inicia o bash dentro do container debian configurado com o dotnet sdk
```

### Estudo Node Z3

```sh
cd estudo-node-z3/projeto;
```

```sh
make dev-run; # npm i && npm run dev-start
```

```sh
make dev-shell; # inicia o bash dentro do container debian configurado com o node
```


## Licença

[MIT - Ladesa e Contribuidores, 2024](./LICENSE).

[MIT - Gabriel R. Antunes, 2024](./LICENSE).
