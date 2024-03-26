using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;

namespace Sisgea.GerarHorario.Core;

public class Gerador
{
    public static (CpModel, List<PropostaAula>) PrepararModelComRestricoes(GerarHorarioOptions options, bool debug = false)
    {
        // =================================
        var model = new CpModel();
        // =================================


        // =================================
        var todasAsPropostasDeAula = new List<PropostaAula>();
        // =================================

        for (int diaSemanaIso = options.DiaSemanaInicio; diaSemanaIso <= options.DiaSemanaFim; diaSemanaIso++)
        {
            for (int intervaloIndex = 0; intervaloIndex < 10; intervaloIndex++)
            {
                foreach (var turma in options.Turmas)
                {
                    foreach (var diario in turma.DiariosDaTurma)
                    {
                        var propostaLabel = $"dia_{diaSemanaIso}::intervalo_{intervaloIndex}::diario_{diario.Id}";

                        var modelBoolVar = model.NewBoolVar(propostaLabel);

                        var propostaDeAula = new PropostaAula(diaSemanaIso, intervaloIndex, diario.Id, modelBoolVar);

                        todasAsPropostasDeAula.Add(propostaDeAula);

                        if (debug)
                        {
                            Console.WriteLine($"--> init proposta de aula | {propostaLabel}");
                        }
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

        // ==========================================================================================================

        // TODO: todas as restrições são implementadas aqui.

        // ...

        // ==========================================================================================================

        return (model, todasAsPropostasDeAula);
    }

    public static IEnumerable<HorarioGerado> GerarHorario(
      GerarHorarioOptions options,
      bool verbose = false)
    {
        // CRIA UM MODELO COM AS RESTRIÇÕES VINDAS DAS OPÇÕES
        var (model, todasAsPropostasDeAula) = PrepararModelComRestricoes(options, verbose);

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

        // ====================================================================================

        // TODO: gerar mais de um horário com CpSolverSolutionCallback OnSolutionCallback

        // ....

        // ==================

        // Filtrar propostasDeAula com modelBoolVar == true na solução atual

        var propostasAtivas = from propostaAula in todasAsPropostasDeAula
                              where
                                  solver.BooleanValue(propostaAula.modelBoolVar)
                              select new HorarioGeradoAula
                              {
                                  Diario = propostaAula.diarioId,
                                  DiaDaSemanaIso = propostaAula.diaSemanaIso,
                                  IntervaloDeTempo = propostaAula.intervaloIndex
                              };

        var horarioGerado = new HorarioGerado
        {
            Aulas = propostasAtivas.ToArray()
        };

        yield return horarioGerado;

        // ====================================================================================

    }
}

