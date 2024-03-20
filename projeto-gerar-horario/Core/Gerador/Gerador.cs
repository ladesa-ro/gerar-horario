using Google.OrTools.Sat;

namespace Core;

class PropostaAula(
    int diaSemanaIso,
    int intervaloIndex,
    IEntidadeIdentificacao diarioId,
    BoolVar modelBoolVar
    )
{
    public int diaSemanaIso = diaSemanaIso;
    public int intervaloIndex = intervaloIndex;
    public IEntidadeIdentificacao diarioId = diarioId;
    public BoolVar modelBoolVar = modelBoolVar;
}

public class Gerador
{
    public static CpModel PrepararModelComRestricoes(IGerarHorarioOptions options, bool debug = true)
    {
        Console.WriteLine(options);

        var model = new CpModel();

        var todasAsPropostasDeAula = new List<PropostaAula>();

        int diasDeTrabalho = options.DiaSemanaFim + 1 - options.DiaSemanaInicio;
        int totalIntervalos = options.IntervalosDeAula.Length;

        int totalDiarios = 15 * 15;

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

        for (int diaSemanaIso = options.DiaSemanaInicio; diaSemanaIso <= options.DiaSemanaFim; diaSemanaIso++)
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

                    var propostaAula = new PropostaAula(diaSemanaIso, intervaloIndex, EntidadeIdentificacao.Id(diarioId), boolVar);
                    todasAsPropostasDeAula.Add(propostaAula);

                    storeBoolVars[diaSemanaIso, intervaloIndex, diarioId] = boolVar;
                }
            }
        }

        // para cada turma

        foreach (var turma in options.Turmas)
        {
            foreach (var diaSemanaIso in Enumerable.Range(options.DiaSemanaInicio, options.DiaSemanaFim))
            {
                foreach (var intervaloIndex in Enumerable.Range(0, options.IntervalosDeAula.Length))
                {
                    var propostas = from propostaAula in todasAsPropostasDeAula
                                    where
                                       propostaAula.diaSemanaIso == diaSemanaIso
                                       && propostaAula.intervaloIndex == intervaloIndex
                                       && turma.DiariosDaTurma.Any(diario => diario.Id == propostaAula.diarioId)
                                    select propostaAula.modelBoolVar;

                    model.AddAtMostOne(propostas);
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

