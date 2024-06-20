# gerar-horario

[![CodeQL][badge-codeql-src]][badge-codeql-href]
[![NUnit Tests + Allure Reports][badge-tests-src]][badge-tests-href]

Allure Report: [clique aqui][tests-allure-report].

## Ambientes

### Produção

[![CI Production][action-ci-prod-src]][action-ci-prod-href]

### Desenvolvimento

[![CI Development][action-ci-dev-src]][action-ci-dev-href]

## Configuração Local

```sh
git clone https://github.com/ladesa-ro/gerar-horario.git
cd gerar-horario
```

### Projeto Gerar Horário

```sh
dotnet test;
```

Scripts dev: Veja [./Makefile](./Makefile).

### Playground (com jupyter e .net interactive)

```sh
cd gerar-horario/Playground;
```

- Notebooks Jupyter: [`./Playground/notebooks`](./Playground/notebooks);

Continue em [`./Playground/README.md`](./Playground/README.md).

### Estudo

```sh
git checkout estudo-ts;
```

- Estudo Node Z3: [`tree/estudo-ts/estudo/estudo-node-z3/projeto`][estudo-node-z3];
- Estudo CSharp Or-Tools: [`tree/estudo-ts/estudo/estudo-csharp/dotnet-project`][estudo-csharp-ortools];

Continue em [`tree/estudo-ts/README.md`][estudo].

## Licença

[MIT - Gabriel R. Antunes, 2024](./LICENSE).

[MIT - Ladesa e Contribuidores, 2024](./LICENSE).

<!-- Badges -->

[badge-codeql-src]: https://github.com/ladesa-ro/gerar-horario/actions/workflows/github-code-scanning/codeql/badge.svg
[badge-codeql-href]: https://github.com/ladesa-ro/gerar-horario/actions/workflows/github-code-scanning/codeql
[badge-tests-src]: https://github.com/ladesa-ro/gerar-horario/actions/workflows/test-and-deploy.yml/badge.svg
[badge-tests-href]: https://github.com/ladesa-ro/gerar-horario/actions/workflows/test-and-deploy.yml
[tests-allure-report]: https://ladesa-ro.github.io/gerar-horario

<!-- Badges / Actions / Production  -->

[action-ci-prod-src]: https://img.shields.io/github/actions/workflow/status/ladesa-ro/gerar-horario/ci.yml?style=flat&logo=github&logoColor=white&label=CI@production&branch=production&labelColor=18181B
[action-ci-prod-href]: https://github.com/ladesa-ro/gerar-horario/actions/workflows/ci.yml?query=branch%3Aproduction

<!-- Badges / Actions / Development  -->

[action-ci-dev-src]: https://img.shields.io/github/actions/workflow/status/ladesa-ro/gerar-horario/ci.yml?style=flat&logo=github&logoColor=white&label=CI@development&branch=development&labelColor=18181B
[action-ci-dev-href]: https://github.com/ladesa-ro/gerar-horario/actions/workflows/ci.yml?query=branch%3Adevelopment

<!-- Estudo -->

[estudo]: https://github.com/ladesa-ro/gerar-horario/blob/estudo-ts/README.md
[estudo-node-z3]: https://github.com/ladesa-ro/gerar-horario/tree/estudo-ts/estudo/estudo-node-z3/projeto
[estudo-csharp-ortools]: https://github.com/ladesa-ro/gerar-horario/tree/estudo-ts/estudo/estudo-csharp/dotnet-project
