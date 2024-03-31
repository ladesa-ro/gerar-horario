using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;

namespace Sisgea.GerarHorario.Core;

public class Gerador
{

    public static IEnumerable<HorarioGerado> GerarHorario(
      GerarHorarioOptions options)
    {
        // CRIA UM MODELO COM AS RESTRIÇÕES VINDAS DAS OPÇÕES
        var contexto = PrepararModelComRestricoes(options);


        // ==============================================================

        // evento thread de solução iniciado
        var tickThreadStarted = new AutoResetEvent(false);

        // evento horário gerado
        var tickGenerated = new AutoResetEvent(false);

        // evento permissão para gerar o próximo horário
        var tickGenerateNext = new AutoResetEvent(false);

        HorarioGerado? horarioGerado = null;

        // thread de solução de horário para essa requisição
        var solutionGeneratorThread = new Thread(() =>
        {
            Console.WriteLine("==> [thread de solução] | iniciado");
            tickThreadStarted.Set();

            Console.WriteLine("==> [thread de solução] | aguardando tickGenerateNext para iniciar o solver.Solve");
            tickGenerateNext.WaitOne();

            Console.WriteLine("==> [thread de solução] | recebeu permissão para iniciar a geração");

            long? previousScore = null;

            do
            {
                var solver = new CpSolver
                {
                    StringParameters = "enumerate_all_solutions:true"
                };

                var solutionPrinter = new GeradorSolutionCallback(contexto, (spHorarioGerado) =>
                {
                    horarioGerado = spHorarioGerado;
                    Console.WriteLine("==> [thread de solução] | gerou um horário");

                    Console.WriteLine("==> [thread de solução] | disparando tickGenerated");
                    tickGenerated.Set();

                    Console.WriteLine("==> [thread de solução] | aguardando permissão para continuar a geração");
                    tickGenerateNext.WaitOne();
                    Console.WriteLine("==> [thread de solução] | recebeu permissão para continuar a geração");
                });

                if (previousScore != null)
                {
                    Gerador.OtimizarResultadoDeAcordoComAsPreferencias(contexto, previousScore - 1);
                }

                var sat = solver.Solve(contexto.Model, solutionPrinter);

                Console.WriteLine($"==> [thread de solução] | sat: {sat}");

                if (sat == CpSolverStatus.Feasible || sat == CpSolverStatus.Optimal)
                {
                    var solverScore = solver.ObjectiveValue;
                    Console.WriteLine($"==> [thread de solução] | solverScore: {solverScore}");

                    previousScore = (long)solverScore;
                }
                else
                {
                    previousScore = 0;
                }



                Console.WriteLine("");
                Console.WriteLine("============================");
                Console.WriteLine("Statistics");
                // STATUS DA SOLUÇÃO
                Console.WriteLine($"Status de solução: {sat}");
                // Mostra a solução.
                // Check that the problem has a feasible solution.
                if (sat == CpSolverStatus.Optimal || sat == CpSolverStatus.Feasible)
                {
                    Console.WriteLine($"  - Score do horário: {solver.ObjectiveValue}");
                }
                else
                {
                    Console.WriteLine("  - Score do horário: No solution found.");
                }
                Console.WriteLine($"  - conflicts : {solver.NumConflicts()}");
                Console.WriteLine($"  - branches  : {solver.NumBranches()}");
                Console.WriteLine($"  - wall time : {solver.WallTime()}s");
                Console.WriteLine("============================");
                Console.WriteLine("");
            } while (previousScore > 0);

            Console.WriteLine("==> [thread de solução] | terminou a geração de todas as soluções possíveis");

            horarioGerado = null;
            tickGenerated.Set();
        });


        solutionGeneratorThread.Start();

        Console.WriteLine("=> Gerador#gerarHorario | aguarda o thread de solução iniciar");
        tickThreadStarted.WaitOne();
        Console.WriteLine("=> Gerador#gerarHorario | thread de solução iniciou");

        do
        {
            Console.WriteLine("=> Gerador#gerarHorario | permite a geração do próximo horário");
            tickGenerateNext.Set();

            Console.WriteLine("=> Gerador#gerarHorario | aguardando a geração do próximo horário");
            tickGenerated.WaitOne();

            if (horarioGerado != null)
            {
                Console.WriteLine("=> Gerador#gerarHorario | geração do próximo horário recebida");
                yield return horarioGerado;
            }
            else
            {
                Console.WriteLine("=> Gerador#gerarHorario | recebeu um horário nulo. bye");
            }

        } while (horarioGerado != null);

        yield break;
    }


    ///<summary>
    /// Ponto de partida que inicia, restringe e otimizar o modelo para
    /// solucionar o problema da geração de horário.
    ///</summary>
    public static GerarHorarioContext PrepararModelComRestricoes(GerarHorarioOptions options)
    {
        // ================================================
        var contexto = new GerarHorarioContext(options, iniciarTodasAsPropostasDeAula: true);
        // ================================================

        // contexto.Model -> Google.OrTools.Sat.CpModel;
        // contexto.Options -> GerarHorarioOptions;
        // contexto.TodasAsPropostasDeAula -> List<PropostaDeAula>;

        // ======================================
        Restricoes.AplicarLimiteDeNoMaximoUmDiarioAtivoPorTurmaEmUmHorario(contexto);
        // ==========================================================================================================
        Restricoes.AplicarLimiteDeNoMaximoUmDiarioAtivoPorProfessorEmUmHorario(contexto);
        // ==========================================================================================================
        Restricoes.AplicarLimiteDeDiarioNaSemana(contexto);
        // ==========================================================================================================
        // TODO: mais restrições serão implementadas aqui.
        // ==========================================================================================================
        // ...
        // ==========================================================================================================

        Gerador.OtimizarResultadoDeAcordoComAsPreferencias(contexto);

        // ==========================================================================================================

        return contexto;
    }

    ///<summary>
    /// Visto que podem haver várias soluções válidas possíveis, precisamos
    /// otimizar a resposta para que seja a mais satisfatória possível de
    /// acordo com as preferências de agrupamento da turma e preferências
    /// de cada professor.
    ///</summary>
    public static void OtimizarResultadoDeAcordoComAsPreferencias(GerarHorarioContext contexto, long? limiteScore = null)
    {
        var score = LinearExpr.NewBuilder();

        foreach (var propostaDeAula in contexto.TodasAsPropostasDeAula)
        {
            score.AddTerm((IntVar)propostaDeAula.ModelBoolVar, 1);
        }

        if (limiteScore != null)
        {
            contexto.Model.Add(score <= contexto.Model.NewConstant((long)limiteScore));
        }

        contexto.Model.Maximize(score);
    }

}

