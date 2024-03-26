using Core.Dtos.Configuracoes;
using Core.Dtos.HorarioGerado;
using Google.OrTools.Sat;

namespace Core.Gerador;

public class Gerador
{
    public static CpModel PrepararModelComRestricoes(GerarHorarioOptions options, bool debug = true)
    {
        // =================================
        var model = new CpModel();
        // =================================


        // =================================
        var todasAsPropostasDeAula = new List<PropostaAula>();
        // =================================

        // TODO: implementar corretamente com options
        int totalDeDiarios = 15 * 15;

        // ====================================================================

        for (int diaSemanaIso = options.DiaSemanaInicio; diaSemanaIso <= options.DiaSemanaFim; diaSemanaIso++)
        {
            for (int intervaloIndex = 0; intervaloIndex < 10; intervaloIndex++)
            {
                for (int diarioId = 0; diarioId < totalDeDiarios; diarioId++)
                {
                    var label = $"dia_{diaSemanaIso}::intervalo_{intervaloIndex}::diario_{diarioId}";
                    var modelBoolVar = model.NewBoolVar(label);

                    var propostaDeAula = new PropostaAula(diaSemanaIso, intervaloIndex, Convert.ToString(diarioId), modelBoolVar);
                    todasAsPropostasDeAula.Add(propostaDeAula);

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
                foreach (var intervaloIndex in Enumerable.Range(0, options.HorariosDeAula.Length))
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

    public static IEnumerable<HorarioGerado> GerarHorario(
      GerarHorarioOptions options,
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

        // TODO: Incluir propostasDeAula com (modelBoolVar == true) em  para HorarioGerado#HorarioGeradoAula[]
        var horarioGerado = new HorarioGerado { };
        yield return horarioGerado;

    }
}

