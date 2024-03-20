using Google.OrTools.Sat;

namespace Core;

public class Gerador
{
    public static CpModel PrepararModelComRestricoes(IGerarHorarioOptions gerarHorarioOptions, bool debug = true)
    {
        Console.WriteLine(gerarHorarioOptions);

        var model = new CpModel();

        int diasDeTrabalho = ((int)gerarHorarioOptions.DiaFim) + 1 - ((int)gerarHorarioOptions.DiaInicio);
        int totalIntervalos = 10;
        int totalDiarios = 1;

        if (debug)
        {
            Console.WriteLine($"Dias de trabalho: {diasDeTrabalho} - Total intervalos: {totalIntervalos} - Total diários: {totalDiarios}.");
        }

        BoolVar[,,] storeBoolVars = new BoolVar[7, totalIntervalos, totalDiarios];

        if (debug)
        {
            Console.WriteLine($"Tamanho da matriz de booleans: {7 * totalIntervalos * totalDiarios}");
            Console.WriteLine($"Tamanho da matriz de booleans (apenas para os dias de trabalho): {diasDeTrabalho * totalIntervalos * totalDiarios}");
        }

        for (int diaSemanaIso = (int)gerarHorarioOptions.DiaInicio; diaSemanaIso <= (int)gerarHorarioOptions.DiaFim; diaSemanaIso++)
        {
            for (int intervaloIndex = 0; intervaloIndex < 10; intervaloIndex++)
            {
                for (int diarioId = 0; diarioId < totalDiarios; diarioId++)
                {
                    var boolVarLabel = $"dia_{diaSemanaIso}::intervalo_{intervaloIndex}::diario_{diarioId}";

                    if (debug)
                    {
                        Console.WriteLine($"--> init bool var | {boolVarLabel}");
                    }

                    var boolVar = model.NewBoolVar(boolVarLabel);
                    storeBoolVars[diaSemanaIso, intervaloIndex, diarioId] = boolVar;
                }
            }
        }

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

