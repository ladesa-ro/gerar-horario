import { addMinutes, format, parse } from "date-fns";
import { isNil } from "lodash";
import { Arith, init } from "z3-solver";

const parseTime = (time: string) => parse(time, "HH:mm", new Date());
const formatTime = (time: Date | number | string) => format(time, "HH:mm");

export enum DiaSemanaIso {
  DOMINGO = 0,
  SEGUNDA = 1,
  TERCA = 2,
  QUARTA = 3,
  QUINTA = 4,
  SEXTA = 5,
  SABADO = 6,
}

type IDiario = {
  id: number;

  disciplina: {
    nome: string;
  };

  quantidadeMaximaSemana: number;

  professor: {
    id: IProfessor["id"];
  };
};

type ITempo = {
  inicio: string;
  fim: string;
};

type IDisponibilidade = {
  diaSemanaIso: number;
  inicio: string;
  fim: string;
};

type IProfessor = {
  id: number;

  nome: string;

  disponibilidades: IDisponibilidade[];
};

type ITurma = {
  id: number;
  nome: string;
  diarios: IDiario[];
  disponibilidades: IDisponibilidade[];
};

type IGerarHorarioInput = {
  diaInicio: DiaSemanaIso;
  diaFim: DiaSemanaIso;

  tempos: ITempo[];

  turmas: ITurma[];
  professores: IProfessor[];
};

type IDiaTempoDiario = {
  diaSemana: number;
  indexTempo: number;
  idDiario: number;
  solve: Arith;
};

export function* gerarTempos(horaInicio: string, duracaoMinutos: number, quantidade: number): Iterable<ITempo> {
  const addOffset = (hora: string, offset: number) => formatTime(addMinutes(parseTime(hora), duracaoMinutos * offset));

  for (let offset = 0; offset < quantidade; offset++) {
    yield {
      inicio: addOffset(horaInicio, offset),
      fim: addOffset(horaInicio, offset + 1),
    };
  }
}

class StoreDiaTempoDiario {
  #storeDiaTempoDiario: IDiaTempoDiario[] = [];

  add(diaSemana: number, indexTempo: number, idDiario: number, solve: Arith) {
    this.#storeDiaTempoDiario.push({
      diaSemana,
      indexTempo,
      idDiario,
      solve,
    });
  }

  filter<T = IDiaTempoDiario>(
    options?: Partial<Omit<IDiaTempoDiario, "solve">> | null,
    customFilter?: ((i: IDiaTempoDiario) => boolean) | null,
    map = (value: IDiaTempoDiario): T => value as T
  ) {
    return this.#storeDiaTempoDiario
      .filter((i) => {
        if (!isNil(options?.diaSemana) && i.diaSemana !== options.diaSemana) {
          return false;
        }

        if (!isNil(options?.indexTempo) && i.indexTempo !== options.indexTempo) {
          return false;
        }

        if (!isNil(options?.idDiario) && i.idDiario !== options.idDiario) {
          return false;
        }

        if (customFilter && !customFilter(i)) {
          return false;
        }

        return true;
      })
      .map(map);
  }

  log() {}
}

export class GeradorHorario {
  async gerarHorario(options: IGerarHorarioInput) {
    console.log(options);

    const { Context } = await init();

    const Z3 = Context("main");

    const opt = new Z3.Optimize();

    const storeDiaTempoDiario = new StoreDiaTempoDiario();

    function* gerarDiaSemana() {
      for (let diaSemana = options.diaInicio; diaSemana <= options.diaFim; diaSemana++) {
        yield diaSemana;
      }
    }

    function* gerarTempos() {
      for (const tempo of options.tempos) {
        const indexTempo = options.tempos.indexOf(tempo);
        const [tempoInicio, tempoFim] = [parseTime(tempo.inicio), parseTime(tempo.fim)];
        yield { indexTempo, tempo, tempoInicio, tempoFim };
      }
    }

    function* gerarDiarios() {
      for (const turma of options.turmas) {
        for (const diario of turma.diarios) {
          yield {
            diario,
            turma,
            //
          };
        }
      }
    }

    function* gerarDiaTempoDiario() {
      for (let diaSemana of gerarDiaSemana()) {
        for (const tempo of options.tempos) {
          const indexTempo = options.tempos.indexOf(tempo);

          for (const diario of options.turmas.map((turma) => turma.diarios).flat(1)) {
            yield {
              diaSemana,
              //
              tempo,
              indexTempo,
              //
              diario,
              //
            };
          }
        }
      }
    }

    for (const diaTempoDiario of gerarDiaTempoDiario()) {
      const solve = Z3.Int.const(`${diaTempoDiario.diaSemana}_${diaTempoDiario.indexTempo}_${diaTempoDiario.diario.id}`);

      opt.add(solve.eq(0).or(solve.eq(1)));

      storeDiaTempoDiario.add(diaTempoDiario.diaSemana, diaTempoDiario.indexTempo, diaTempoDiario.diario.id, solve);
    }

    storeDiaTempoDiario.log();

    // Garantir apenas 1 diario por diaSemana_periodo por turma

    for (const diaSemana of gerarDiaSemana()) {
      for (const tempo of options.tempos) {
        const [gridAulaInicio, gridAulaFim] = [parseTime(tempo.inicio), parseTime(tempo.fim)];

        const indexTempo = options.tempos.indexOf(tempo);

        for (const turma of options.turmas) {
          const diariosSolves = storeDiaTempoDiario.filter(
            { indexTempo, diaSemana },
            (i) => turma.diarios.findIndex((j) => i.idDiario === j.id) !== -1,
            (i) => i.solve
          );

          opt.add(Z3.Sum(diariosSolves[0], ...diariosSolves.slice(1).map((i) => i)).le(1));

          const turmaTemDisponibilidade = turma.disponibilidades.some((disponibilidade) => {
            const [disponibilidadeInicio, disponibilidadeFim] = [
              parseTime(disponibilidade.inicio),
              parseTime(disponibilidade.fim),
            ];

            if (disponibilidade.diaSemanaIso !== diaSemana) {
              return false;
            }

            if (disponibilidadeInicio <= gridAulaInicio && disponibilidadeFim >= gridAulaFim) {
              return true;
            }

            return false;
          });

          console.log(
            `${turma.nome} - Dia ${diaSemana} - ${tempo.inicio} a ${tempo.fim} |`,
            turmaTemDisponibilidade ? "  disponivel" : "indisponivel"
          );

          if (!turmaTemDisponibilidade) {
            diariosSolves.forEach((i) => {
              opt.add(i.eq(0));
            });
          }
        }
      }
    }

    // Garantir diario.quantidadeMaximaSemana

    for (const { diario } of gerarDiarios()) {
      const solves = storeDiaTempoDiario.filter({ idDiario: diario.id }, null, (i) => i.solve);
      opt.add(Z3.Sum(solves[0], ...solves.slice(1)).le(diario.quantidadeMaximaSemana));
    }

    // Garantir a disponibilidade de um professor

    for (const { diario } of gerarDiarios()) {
      const professor = options.professores.find((professor) => professor.id === diario.professor.id)!;

      for (let diaSemana of gerarDiaSemana()) {
        for (const { indexTempo, tempoInicio, tempoFim, tempo } of gerarTempos()) {
          const professorTemDisponibilidade = professor.disponibilidades.some((disponibilidade) => {
            const [disponibilidadeInicio, disponibilidadeFim] = [
              parseTime(disponibilidade.inicio),
              parseTime(disponibilidade.fim),
            ];

            if (disponibilidade.diaSemanaIso !== diaSemana) {
              return false;
            }

            if (disponibilidadeInicio <= tempoInicio && disponibilidadeFim >= tempoFim) {
              return true;
            }

            return false;
          });

          console.log(
            `${professor.nome} - Dia ${diaSemana} - ${tempo.inicio} a ${tempo.fim} |`,
            professorTemDisponibilidade ? "  disponivel" : "indisponivel"
          );

          if (!professorTemDisponibilidade) {
            const diarioSolves = storeDiaTempoDiario.filter({
              idDiario: diario.id,
              indexTempo: indexTempo,
              diaSemana: diaSemana,
            });

            diarioSolves.forEach((i) => {
              opt.add(i.solve.eq(0));
            });
          }
        }
      }
    }


    // por padrão, um conjunto vazio satisfaz todas as condições.
    // entretanto, queremos o maior número de aulas possíveis

    const qndAulas = Z3.Int.const("qndAulas");

    const allDiaTempoAula = storeDiaTempoDiario.filter(null, null, (i) => i.solve);
    opt.add(qndAulas.eq(Z3.Sum(allDiaTempoAula[0], ...allDiaTempoAula.slice(1))));

    opt.maximize(qndAulas);

    const result = await opt.check();

    if (result === "sat") {
      const model = opt.model();

      for (const diaSemana of gerarDiaSemana()) {
        for (const { indexTempo, tempo } of gerarTempos()) {
          for (const turma of options.turmas) {
            const [solvedDiaTempoDiario] = storeDiaTempoDiario.filter(
              { diaSemana, indexTempo },
              (i) =>
                model.eval(i.solve).toString() === "1" && turma.diarios.findIndex((diario) => diario.id === i.idDiario) !== -1
            );

            const solvedDiario =
              (solvedDiaTempoDiario && turma.diarios.find((diario) => diario.id === solvedDiaTempoDiario.idDiario)) ?? null;

            console.log(
              `${turma.nome} - ${diaSemana} - ${tempo.inicio} a ${tempo.fim} - ${
                (solvedDiario ? solvedDiario?.disciplina?.nome : null) ?? "nem"
              }`
            );
          }
        }
      }
    }

    return true;
  }
}
