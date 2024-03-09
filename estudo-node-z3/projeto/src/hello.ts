import type { Arith, Model } from "z3-solver";
import { init } from "z3-solver";
import { allSolutions } from "./allSolutions";

type IGerarHorarioOptions = {
  dias: number;
  intervalos: [string, string][];
};

function mostrarHorario(options: IGerarHorarioOptions, model: Model, matrizDimensionalAulas: Arith<"main">[][]) {
  for (let linha = 0; linha < options.intervalos.length; linha++) {
    for (let coluna = 0; coluna < options.dias; coluna++) {
      process.stdout.write(`${String(model.eval(matrizDimensionalAulas[coluna][linha])).padStart(8, " ")} `);
    }
    process.stdout.write(`\n`);
  }

}

async function gerarHorario(options: IGerarHorarioOptions) {
  console.clear()

  const { Context } = await init();

  const Z3 = Context("main");
  const { Solver } = Z3;

  const Aula = Z3.Int.sort();

  const matrizDimensionalAulas = Array.from({ length: options.dias }).map((_, dia) =>
    Array.from({ length: options.intervalos.length }).map((_, intervalo) => Z3.Const(`c_${dia}_${intervalo}`, Aula))
  );

  let solver = new Solver();

  matrizDimensionalAulas.flat(1).forEach((cell) => {
    solver.add(cell.gt(0));
    solver.add(cell.neq(0));
  });

  solver.add(matrizDimensionalAulas[0][0].neq(matrizDimensionalAulas[0][1]));

  let count = 0;

  for await (const model of allSolutions(...solver.assertions())) {
    mostrarHorario(options, model, matrizDimensionalAulas);
    process.stdout.write(`\n`);

    if (++count > 3) {
      break;
    }
  }
}

gerarHorario({
  dias: 5,
  intervalos: [
    //
    ["07:30", "08:20"],
    ["08:20", "09:10"],
    ["09:10", "10:00"],
    ["10:20", "11:10"],
    ["11:10", "12:00"],
  ],
});
