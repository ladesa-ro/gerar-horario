import type { Bool, Model } from "z3-solver";

/**
 * Generate all possible solutions from given assumptions.
 *
 * **NOTE**: The set of solutions might be infinite.
 * Always ensure to limit amount generated, either by knowing that the
 * solution space is constrained, or by taking only a specified
 * amount of solutions
 * ```typescript
 * import { sliceAsync } from 'iter-tools';
 * // ...
 * for await (const model of sliceAsync(10, solver.solutions())) {
 *
 * }
 * ```
 * @see http://theory.stanford.edu/~nikolaj/programmingz3.html#sec-blocking-evaluations
 * @returns Models with solutions. Nothing if no constants provided
 */
// TODO(ritave): Use faster solution https://stackoverflow.com/a/70656700
// TODO(ritave): Move to high-level.ts
export async function* allSolutions<Name extends string>(...assertions: Bool<Name>[]): AsyncIterable<Model<Name>> {
  if (assertions.length === 0) {
    return;
  }

  const { Or } = assertions[0].ctx;
  const solver = new assertions[0].ctx.Solver();
  solver.add(...assertions);

  while ((await solver.check()) === "sat") {
    const model = solver.model();
    const decls = model.decls();
    if (decls.length === 0) {
      return;
    }
    yield model;

    solver.add(
      Or(
        ...decls
          // TODO(ritave): Assert on arity > 0
          .filter((decl) => decl.arity() === 0)
          .map((decl) => {
            const term = decl.call();
            // TODO(ritave): Assert not an array / uninterpreted sort
            const value = model.eval(term, true);
            return term.neq(value);
          })
      )
    );
  }
}
