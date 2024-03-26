import { expect, test } from "vitest";
import { DiaSemanaIso } from "../../models/DiaSemanaIso";
import { gerarIntervalos } from "../../utils/generators/gerarIntervalos";
import { GeradorHorario } from "./GeradorHorario";

test("adds 1 + 2 to equal 3", async () => {
  const gerador = new GeradorHorario();

  const horario = gerador.gerarHorario({
    diaInicio: DiaSemanaIso.SEGUNDA,
    diaFim: DiaSemanaIso.SEXTA,
    //

    tempos: [
      //
      ...gerarIntervalos("07:30", 50, 3), // 07:30 - 08:20 ||||| 08:20 - 09:10  |||| 09:10 - 10:00
      ...gerarIntervalos("10:20", 50, 2), // 10:20 - 11:10  |||| 11:10 - 12:00
      //
      ...gerarIntervalos("13:00", 50, 3),
      ...gerarIntervalos("15:50", 50, 2),
      //
    ],

    //
    professores: [
      {
        id: 1,
        nome: "Danilo",
        disponibilidades: [
          {
            diaSemanaIso: DiaSemanaIso.SEGUNDA,
            inicio: "07:30",
            fim: "12:00",
          },
          {
            diaSemanaIso: DiaSemanaIso.SEGUNDA,
            inicio: "13:30",
            fim: "17:30",
          },
        ],
      },
    ],

    turmas: [
      {
        id: 1,
        nome: "1A INF",

        disponibilidades: [
          {
            diaSemanaIso: DiaSemanaIso.SEGUNDA,
            inicio: "13:00",
            fim: "17:30",
          },
        ],

        diarios: [
          {
            id: 1,

            disciplina: {
              nome: "Redes",
            },

            quantidadeMaximaSemana: 2,

            professor: {
              id: 1,
            },
          },
        ],
      },
    ],
  });

  await expect(horario).resolves.toBeDefined();
});
