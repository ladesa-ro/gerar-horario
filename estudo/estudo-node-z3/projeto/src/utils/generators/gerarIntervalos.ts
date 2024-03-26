import { addMinutes } from "date-fns";
import { ITempo } from "../../models";
import { formatTime } from "../date-time/formatTime";
import { parseTime } from "../date-time/parseTime";

const addMinutosOffset = (hora: string, minutos: number, offset: number) =>
  formatTime(addMinutes(parseTime(hora), minutos * offset));

export function* gerarIntervalos(
  horaInicio: string,
  duracaoMinutos: number,
  quantidade: number,
): Iterable<ITempo> {
  for (let offset = 0; offset < quantidade; offset++) {
    yield {
      inicio: addMinutosOffset(horaInicio, duracaoMinutos, offset),
      fim: addMinutosOffset(horaInicio, duracaoMinutos, offset + 1),
    };
  }
}
