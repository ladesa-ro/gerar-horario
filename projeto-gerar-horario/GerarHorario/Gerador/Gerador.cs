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
            for (int intervaloIndex = 0; intervaloIndex < options.HorariosDeAula.Length; intervaloIndex++)
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
                                       && turma.DiariosDaTurma.Any(diario => diario.Id == propostaAula.diarioId)
                                    select propostaAula.modelBoolVar;


                    var propostasList = propostas.ToList();

                    Console.WriteLine($"Turma: {turma.Id} | Dia: {diaSemanaIso} | Intervalo: {intervaloIndex} | Propostas: {propostasList.Count}");

                    model.AddAtMostOne(propostasList);
                }
            }
        }

        // ==========================================================================================================

        // RESTRIÇÃO: Diário: quantidade máxima na semana

        foreach (var turma in options.Turmas)
        {
            foreach (var diario in turma.DiariosDaTurma)
            {
                var propostasDoDiario = from propostaAula in todasAsPropostasDeAula
                                        where
                                            propostaAula.diarioId == diario.Id
                                        select propostaAula.modelBoolVar;

                model.Add(LinearExpr.Sum(propostasDoDiario) <= diario.QuantidadeMaximaSemana);
            }
        }

        // ==========================================================================================================

        // TODO: todas as restrições são implementadas aqui.

        // ...

        // ==========================================================================================================

        LinearExprBuilder score = LinearExpr.NewBuilder();

        foreach (var propostaDeAula in todasAsPropostasDeAula)
        {
            score.AddTerm((IntVar)propostaDeAula.modelBoolVar, 1);
        }

        model.Maximize(score);

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
                              select new HorarioGeradoAula(propostaAula.diarioId, propostaAula.intervaloIndex, propostaAula.diaSemanaIso);

        var horarioGerado = new HorarioGerado
        {
            Aulas = propostasAtivas.ToArray()
        };

        yield return horarioGerado;

        // ====================================================================================

    }
}

