using Google.OrTools.Sat;

namespace Core;

public class Gerador
{
    public static CpModel PrepararModelComRestricoes(IGerarHorarioOptions gerarHorarioOptions)
    {
        Console.WriteLine(gerarHorarioOptions);

        CpModel model = new CpModel();

        // TODO: todas as restrições serão implementadas aqui.

        return model;
    }

    public static IEnumerable<IHorarioGerado> GerarHorario(
      IGerarHorarioOptions gerarHorarioOptions,
      bool verbose = true)
    {
        // CRIA UM MODELO COM AS RESTRIÇÕES VINDAS DAS OPÇÕES
        CpModel model = PrepararModelComRestricoes(gerarHorarioOptions);

        // RESOLVE O MODELO
        CpSolver solver = new CpSolver();
        CpSolverStatus status = solver.Solve(model);

        // STATUS DA SOLUÇÃO
        Console.WriteLine($"Solve status: {status}");

        // Mostra a solução.
        // Check that the problem has a feasible solution.
        if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
        {
            Console.WriteLine($"Total cost: {solver.ObjectiveValue}\n");
        }
        else
        {
            Console.WriteLine("No solution found.");
        }

        if (verbose)
        {
            Console.WriteLine("Statistics");
            Console.WriteLine($"  - conflicts : {solver.NumConflicts()}");
            Console.WriteLine($"  - branches  : {solver.NumBranches()}");
            Console.WriteLine($"  - wall time : {solver.WallTime()}s");
        }

        var horarioGerado = new HorarioGerado { };

        yield return horarioGerado;
        yield return horarioGerado;
        yield return horarioGerado;
    }
}

