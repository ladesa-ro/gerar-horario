import { isNil } from "lodash";
import { Arith } from "z3-solver";
import { IDiaTempoDiario } from "../../models";

export class StoreDiaTempoDiario {
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
    map = (value: IDiaTempoDiario): T => value as T,
  ) {
    return this.#storeDiaTempoDiario
      .filter((i) => {
        if (!isNil(options?.diaSemana) && i.diaSemana !== options.diaSemana) {
          return false;
        }

        if (
          !isNil(options?.indexTempo) &&
          i.indexTempo !== options.indexTempo
        ) {
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
