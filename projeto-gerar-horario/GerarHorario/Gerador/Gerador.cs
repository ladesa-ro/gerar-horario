using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;

namespace Sisgea.GerarHorario.Core;

public class Gerador
{
    ///<summary>
    /// Visto que podem haver várias soluções válidas possíveis, precisamos  
    /// otimizar a resposta para que seja a mais satisfatória possível de
    /// acordo com as preferências de agrupamento da turma e preferências
    /// de cada professor.
    ///</summary>
    public static void OtimizarResultadoDeAcordoComAsPreferencias(GerarHorarioContext contexto)
    {
        LinearExprBuilder score = LinearExpr.NewBuilder();

        foreach (var propostaDeAula in contexto.TodasAsPropostasDeAula)
        {
            score.AddTerm((IntVar)propostaDeAula.ModelBoolVar, 1);
        }

        contexto.Model.Maximize(score);
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

    public static IEnumerable<HorarioGerado> GerarHorario(
      GerarHorarioOptions options)
    {
        // CRIA UM MODELO COM AS RESTRIÇÕES VINDAS DAS OPÇÕES
        var contexto = PrepararModelComRestricoes(options);

        // Inicia o solucionador para o problema

        var tickThreadStarted = new AutoResetEvent(false);

        var tickGenerated = new AutoResetEvent(false);
        var tickGenerateNext = new AutoResetEvent(false);

        HorarioGerado? horarioGerado = null;

        var solutionGeneratorThread = new Thread(() =>
        {
            Console.WriteLine("==> [thread de solução] | iniciado");
            tickThreadStarted.Set();

            var solver = new CpSolver();
            solver.StringParameters = "enumerate_all_solutions:true";

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

            Console.WriteLine("==> [thread de solução] | aguardando tickGenerateNext para iniciar o solver.Solve");
            tickGenerateNext.WaitOne();

            Console.WriteLine("==> [thread de solução] | recebeu permissão para iniciar a geração");
            solver.Solve(contexto.Model, solutionPrinter);
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


            // Console.WriteLine("");
            // Console.WriteLine("============================");
            // Console.WriteLine("Statistics");
            // // STATUS DA SOLUÇÃO
            // Console.WriteLine($"Status de solução: {status}");

            // // Mostra a solução.
            // // Check that the problem has a feasible solution.
            // if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            // {
            //     Console.WriteLine($"  - Score do horário: {solver.ObjectiveValue}");
            // }
            // else
            // {
            //     Console.WriteLine("  - Score do horário: No solution found.");
            // }

            // Console.WriteLine($"  - conflicts : {solver.NumConflicts()}");
            // Console.WriteLine($"  - branches  : {solver.NumBranches()}");
            // Console.WriteLine($"  - wall time : {solver.WallTime()}s");
            // Console.WriteLine("============================");
            // Console.WriteLine("");


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
}

