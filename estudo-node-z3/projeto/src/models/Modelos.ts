import { Arith } from "z3-solver";
import { DiaSemanaIso } from "./DiaSemanaIso";

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
export type ITempo = {
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
export type IGerarHorarioInput = {
  diaInicio: DiaSemanaIso;
  diaFim: DiaSemanaIso;

  tempos: ITempo[];

  turmas: ITurma[];
  professores: IProfessor[];
};
export type IDiaTempoDiario = {
  diaSemana: number;
  indexTempo: number;
  idDiario: number;
  solve: Arith;
};
