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
        // =================================
        var model = new CpModel();
        // =================================


        // =================================
        var todasAsPropostasDeAula = new List<PropostaAula>();
        // =================================

        // =================================
        int quantidadeDeDias = options.DiaSemanaFim + 1 - options.DiaSemanaInicio;
        int totalDeIntervalos = options.IntervalosDeAula.Length;
        // TODO: implementar corretamente com options
        int totalDeDiarios = 15 * 15;

        // =================================

        BoolVar[,,] storeBoolVars = new BoolVar[7, totalDeIntervalos, totalDeDiarios];

        // =================================

        if (debug)
        {
            Console.WriteLine($"Dias de trabalho: {quantidadeDeDias} - Total intervalos: {totalDeIntervalos} - Total diários: {totalDeDiarios}.");
            Console.WriteLine($"Tamanho da matriz de booleans: {7 * totalDeIntervalos * totalDeDiarios}");
            Console.WriteLine($"Tamanho da matriz de booleans (apenas para os dias de trabalho): {quantidadeDeDias * totalDeIntervalos * totalDeDiarios}");
        }

        // ====================================================================

        for (int diaSemanaIso = options.DiaSemanaInicio; diaSemanaIso <= options.DiaSemanaFim; diaSemanaIso++)
        {
            for (int intervaloIndex = 0; intervaloIndex < 10; intervaloIndex++)
            {
                for (int diarioId = 0; diarioId < totalDeDiarios; diarioId++)
                {
                    var label = $"dia_{diaSemanaIso}::intervalo_{intervaloIndex}::diario_{diarioId}";
                    var modelBoolVar = model.NewBoolVar(label);

                    var propostaDeAula = new PropostaAula(diaSemanaIso, intervaloIndex, EntidadeIdentificacao.Id(diarioId), modelBoolVar);
                    todasAsPropostasDeAula.Add(propostaDeAula);

                    storeBoolVars[diaSemanaIso, intervaloIndex, diarioId] = modelBoolVar;

                    if (debug)
                    {
                        Console.WriteLine($"--> init proposta de aula | {label}");
                    }
                }
            }
        }

        // ======================================

        // RESTRIÇÃO: Garantir no máximo 1 aula em um (dia e intervalo) para cada turma.

        foreach (var turma in options.Turmas)
        {
            foreach (var diaSemanaIso in Enumerable.Range(options.DiaSemanaInicio, options.DiaSemanaFim))
            {
                foreach (var intervaloIndex in Enumerable.Range(0, options.IntervalosDeAula.Length))
                {
                    var propostas = from propostaAula in todasAsPropostasDeAula
                                    where
                                       propostaAula.diaSemanaIso == diaSemanaIso // mesmo dia
                                       && propostaAula.intervaloIndex == intervaloIndex // mesmo horário
                                       && turma.DiariosDaTurma.Any(diario => diario.Id == propostaAula.diarioId) // filtrar todos os diarios dessa turma
                                    select propostaAula.modelBoolVar;

                    model.AddAtMostOne(propostas);
                }
            }
        }

        // ======================================

        // TODO: todas as restrições são implementadas aqui.

        // ==========================================================================================================

        return model;
    }

    public static IEnumerable<IHorarioGerado> GerarHorario(
      IGerarHorarioOptions options,
      bool verbose = true)
    {
        // CRIA UM MODELO COM AS RESTRIÇÕES VINDAS DAS OPÇÕES
        var model = PrepararModelComRestricoes(options);

        // RESOLVE O MODELO
        var solver = new CpSolver();
        var status = solver.Solve(model);

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

        // TODO: gerar mais de um horário com CpSolverSolutionCallback OnSolutionCallback

        // TODO: transofomar propostasDeAula com modelBoolVar == true para HorarioGerado
        var horarioGerado = new HorarioGerado { };
        yield return horarioGerado;

    }
}

